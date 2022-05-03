using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           // Add to process Async Task
using Microsoft.Bot.Connector;          // Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    // Add for Dialog 
using System.Net.Http;                  // Add for internet
using Culture_ChatBot.Helpers;               // Add for CardHelper

using System.Data;                      // Add for DB Connection
using System.Data.SqlClient;            // Add for DB Connection
using Culture_ChatBot.Model;                  // Add for Model


namespace Culture_ChatBot.Dialogs
{
    [Serializable]
    public class OrderDialog : IDialog<string>
    {
        private string strMessage = null;
        //private string strOrder;
        private string strServerUrl = "http://localhost:3984/Images/";

        private string strSQL = "SELECT * FROM Menus";

        List<OrderItem> MenuItems = new List<OrderItem>();  // Create list object

        public async Task StartAsync(IDialogContext context)
        {
            //strMessage = null;
            //strOrder = "[Order Menu List] \n";

            //Called MessageReceivedAsync() without user input message
            await this.MessageReceivedAsync(context, null);

            //context.Wait(MessageReceivedAsync);
            //return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            if (result != null)
            {
                Activity activity = await result as Activity;

                if (activity.Text.Trim() == "Exit")
                {
                    List<ReceiptItem> receiptItems = new List<ReceiptItem>();
                    Decimal totalPrice = 0;

                    foreach (OrderItem orderItem in MenuItems)
                    {
                        receiptItems.Add(new ReceiptItem()
                        {
                            Title = orderItem.Title,
                            Price = orderItem.Price.ToString("##########"),
                            Quantity = orderItem.Quantity.ToString(),
                        });
                        totalPrice += orderItem.Price;
                    }

                    // Setting query parameter
                    SqlParameter[] para =
                    {
                        new SqlParameter("@TotalPrice", SqlDbType.SmallMoney),
                        new SqlParameter("@UserID", SqlDbType.NVarChar, 50)
                    };

                    para[0].Value = totalPrice;
                    para[1].Value = activity.Id;

                    // Store ordered menu -> Orders table
                    SQLHelper.ExecuteNonQuery("INSERT INTO Orders(TotalPrice, UserID, OrderDate) " +
                                              "VALUES(@TotalPrice, @UserID, GETDATE())", para);

                    DataSet orderNumber = SQLHelper.RunSQL("SELECT MAX(OrderID) FROM Orders " +
                                                           "WHERE UserID = '" + activity.Id + "'");

                    DataRow row = orderNumber.Tables[0].Rows[0];
                    int orderID = (int)row[0];

                    foreach (OrderItem orderItem in MenuItems)
                    {
                        // Setting query parameter
                        SqlParameter[] para2 =
                        {
                            new SqlParameter("@OrderID", SqlDbType.Int),
                            new SqlParameter("@ItemName", SqlDbType.NVarChar),
                            new SqlParameter("@ItemPrice", SqlDbType.SmallMoney),
                            new SqlParameter("@Quantity", SqlDbType.Int)
                        };

                        para2[0].Value = orderID;
                        para2[1].Value = orderItem.Title;
                        para2[2].Value = orderItem.Price;
                        para2[3].Value = orderItem.Quantity;

                        // Store ordered menu -> Items table
                        SQLHelper.ExecuteNonQuery(
                            "INSERT INTO Items(OrderID, ItemName, ItemPrice, Quantity) " +
                            "VALUES(@OrderID, @ItemName, @ItemPrice, @Quantity)", para2);
                    }

                    // Ordered menu output
                    var cardMessage = context.MakeMessage();
                    cardMessage.Attachments.Add(
                        CardHelper.GetReceiptCard("[Ordered Menu List] \n", receiptItems,
                                                   totalPrice.ToString(), "2%", "10%"));

                    MenuItems.Clear();

                    await context.PostAsync(cardMessage);  
                    context.Done("Order Completed");
                }
                else
                {
                    // DB Connection using SQLHelper
                    string strSQL = "SELECT * FROM Menus WHERE MenuID = " + activity.Text;
                    DataSet DB_DS = SQLHelper.RunSQL(strSQL);
                    DataRow row = DB_DS.Tables[0].Rows[0];

                    // Select data -> Insert List
                    MenuItems.Add(new OrderItem
                    {
                        ItemID = (int)row["MenuID"],
                        Title = row["Title"].ToString(),
                        Price = (Decimal)row["Price"],
                        Quantity = 1,
                    });

                    // Show ordered menu
                    string strOrderMenus = "You ordered...\n";
                    foreach (OrderItem orderItem in MenuItems)
                    {
                        strOrderMenus += orderItem.Title + ": " +
                                         orderItem.Price.ToString("##########") + "\n\n";
                    }

                    await context.PostAsync(strOrderMenus);

                    context.Wait(this.MessageReceivedAsync);


                    //strMessage = string.Format("You ordered {0}.", activity.Text);
                    //strOrder += activity.Text + "\n";
                    //await context.PostAsync(strMessage); // return our reply to the user

                    //// DB Connection using SQLHelper
                    //DataSet DB_DS = SQLHelper.RunSQL(strSQL + " WHERE MenuID = " + activity.Text);
                    //DataRow row = DB_DS.Tables[0].Rows[0];

                    //await context.PostAsync(strMessage);    // return our reply to the user

                    //context.Wait(this.MessageReceivedAsync);

                }
            }
            else
            {
                strMessage = "[Food Order Menu] Select the menu you want to order. > ";
                await context.PostAsync(strMessage);    // return our reply to the userC:\Users\wnsgu\source\repos\Culture_ChatBot_02\Dialogs\OrderDialog.cs

                // version 4

                // DB Connection using SQLHelper
                DataSet DB_DS = SQLHelper.RunSQL(strSQL);

                // Menu
                var message = context.MakeMessage();
                foreach (DataRow row in DB_DS.Tables[0].Rows)
                {
                    // Hero Card-01~04 attachment
                    message.Attachments.Add(CardHelper.GetHeroCard(row["Title"].ToString(),
                                            row["Price"].ToString(),
                                            this.strServerUrl + row["Images"].ToString(),
                                            row["Title"].ToString(), row["MenuID"].ToString()));
                }

                message.Attachments.Add(CardHelper.GetHeroCard("Exit food order...", "Exit", null, "Exit Order", "Exit"));

                message.AttachmentLayout = "carousel";  // Setting Menu Layout Format
                await context.PostAsync(message);       // Output message
                context.Wait(this.MessageReceivedAsync);


                //// version 3
                //// DB Connection
                //SqlConnection DB_CON = new SqlConnection(strDBServer);
                //SqlCommand DB_Query = new SqlCommand(strSQL, DB_CON);
                //SqlDataAdapter DB_Adapter = new SqlDataAdapter(DB_Query);

                //DataSet DB_DS = new DataSet();
                //DB_Adapter.Fill(DB_DS);

                //// Menu
                //var message = context.MakeMessage();
                //foreach(DataRow row in DB_DS.Tables[0].Rows)
                //{
                //    // Hero Card-01~04 attachment
                //    message.Attachments.Add(CardHelper.GetHeroCard(row["Title"].ToString(),
                //                            row["Price"].ToString(),
                //                            this.strServerUrl + row["Images"].ToString(),
                //                            row["Title"].ToString(), row["Title"].ToString()));
                //}

                //message.Attachments.Add(CardHelper.GetHeroCard("Exit food order...", "Exit", null, "Exit Order", "Exit"));

                //message.AttachmentLayout = "carousel";  // Setting Menu Layout Format
                //await context.PostAsync(message);       // Output message
                //context.Wait(this.MessageReceivedAsync);



                //// version2
                //// Menu
                //var message = context.MakeMessage();    // Create message

                ////Hero Card-01-04 attachment

                //message.Attachments.Add(CardHelper.GetHeroCard("자장면", "5000원", this.strServerUrl + "menu_01.jpg", "자장면", "옛날 자장면"));
                //message.Attachments.Add(CardHelper.GetHeroCard("짬뽕", "6000원", this.strServerUrl + "menu_02.jpg", "짬뽕", "굴짬뽕"));
                //message.Attachments.Add(CardHelper.GetHeroCard("탕수육", "8000원", this.strServerUrl + "menu_03.jpg", "탕수육", "레몬 탕수육"));
                //message.Attachments.Add(CardHelper.GetHeroCard("ExitOrder", null, null, "Exit", "Exit food order..."));

                //message.AttachmentLayout = "carousel";  // Setting Menu Layout Format

                //await context.PostAsync(message);       // Output message

                //context.Wait(this.MessageReceivedAsync);



                //// version 1
                //// Menu_01 : 자장면
                //List<CardImage> menu01_images = new List<CardImage>();  // Create image object
                //menu01_images.Add(new CardImage() { Url = this.strServerUrl + "menu_01.jpg" });

                //// Create Button-01
                //List<CardAction> menu01_Button = new List<CardAction>();    // Create Button object
                //menu01_Button.Add(new CardAction()
                //{
                //    Title = "자장면",
                //    Value = "자장면",
                //    Type = ActionTypes.ImBack
                //});

                //// Create Hero Card-01
                //HeroCard menu01_Card = new HeroCard()
                //{
                //    Title = "자장면",
                //    Subtitle = "옛날 자장면",
                //    Images = menu01_images,
                //    Buttons = menu01_Button
                //};

                //// Menu_02 : 짬뽕
                //List<CardImage> menu02_images = new List<CardImage>();  // Create image object
                //menu02_images.Add(new CardImage() { Url = this.strServerUrl + "menu_02.jpg" });

                //// Create Button-02
                //List<CardAction> menu02_Button = new List<CardAction>();    // Create Button object
                //menu02_Button.Add(new CardAction()
                //{
                //    Title = "짬뽕",
                //    Value = "짬뽕",
                //    Type = ActionTypes.ImBack
                //});

                //// Create Hero Card-02
                //HeroCard menu02_Card = new HeroCard()
                //{
                //    Title = "짬뽕",
                //    Subtitle = "굴짬뽕",
                //    Images = menu02_images,
                //    Buttons = menu02_Button
                //};

                //// Menu_03 : 탕수육
                //List<CardImage> menu03_images = new List<CardImage>();  // Create image object
                //menu03_images.Add(new CardImage() { Url = this.strServerUrl + "menu_03.jpg" });

                //// Create Button-03
                //List<CardAction> menu03_Button = new List<CardAction>();    // Create Button object
                //menu03_Button.Add(new CardAction()
                //{
                //    Title = "탕수육",
                //    Value = "탕수육",
                //    Type = ActionTypes.ImBack
                //});

                //// Create Hero Card-03
                //HeroCard menu03_Card = new HeroCard()
                //{
                //    Title = "탕수육",
                //    Subtitle = "레몬 탕수육",
                //    Images = menu03_images,
                //    Buttons = menu03_Button
                //};

                //// Create Button-04
                //List<CardAction> menu04_Button = new List<CardAction>();    // Create Button object
                //menu04_Button.Add(new CardAction()
                //{
                //    Title = "Exit Order",
                //    Value = "Exit",
                //    Type = ActionTypes.ImBack
                //});

                //// Create Hero Card-04
                //HeroCard menu04_Card = new HeroCard()
                //{
                //    Title = "Exit food order...",
                //    Subtitle = null,
                //    Buttons = menu04_Button
                //};

                //var message = context.MakeMessage();                    // Create message
                //message.Attachments.Add(menu01_Card.ToAttachment());    // Hero Card-01 attachment
                //message.Attachments.Add(menu02_Card.ToAttachment());    // Hero Card-02 attachment
                //message.Attachments.Add(menu03_Card.ToAttachment());    // Hero Card-03 attachment
                //message.Attachments.Add(menu04_Card.ToAttachment());    // Hero Card-04 attachment
                //await context.PostAsync(message);

                //context.Wait(this.MessageReceivedAsync);
            }


            //Activity activity = await result as Activity;

            //if(activity.Text.Trim() == "Exit")
            //{
            //    await context.PostAsync(strOrder);  // return our reply to the user
            //    strOrder = null;
            //    context.Done("Order Completed");
            //}
            //else
            //{
            //    strMessage = string.Format("You ordered {0}.", activity.Text);
            //    strOrder += activity.Text + "\n";
            //    await context.PostAsync(strMessage);    // return our reply to the user

            //    context.Wait(MessageReceivedAsync);
            //}
        }
    }
}