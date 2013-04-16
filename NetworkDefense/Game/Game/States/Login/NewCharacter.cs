using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using System;
using Game.Sprites;
using DBCommService;
using System.Text.RegularExpressions;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.States
{
    /// <summary>
    /// Create a new character with a name and gender, and five body factors.
    /// </summary>
    class NewCharacter:TPState
    {
        /// <summary>
        /// Gender Enum: m = male, f = female. 
        /// </summary>
        public enum Gender{ m, f };
        /// <summary>
        /// The width of the screen
        /// </summary>
        private int screenWidth = TPEngine.Get().ScreenSize.Width;
        /// <summary>
        /// The height of the screen
        /// </summary>
        private int screenHeight = TPEngine.Get().ScreenSize.Height;
        /// <summary>
        /// The array of layer of the state
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The number of the layer on the state
        /// </summary>
        private Int16 NumLayers = 4;
        /// <summary>
        /// Mouse cursor
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(32, 32));

        /// <summary>
        /// The height of the background box of the character image
        /// </summary>
        private const int charBoxHeight = 250;
        /// <summary>
        /// The width of the background box of the character image
        /// </summary>
        private const int charBoxWidth = 320;
        /// <summary>
        /// The colour of the background boxes
        /// </summary>
        private Color boxColor = Color.SteelBlue;
        /// <summary>
        /// The x position of the background box of the character information
        /// </summary>
        private const int charInfoX = 20;
        /// <summary>
        /// The height of the background box of the bodyparts section
        /// </summary>
        private const int bodyPartsBoxHeight = 400;
        /// <summary>
        /// The height of the background box of the character image 
        /// </summary>
        private const int charImageBoxHeight = 600;
        /// <summary>
        /// The width of the background box of the character image
        /// </summary>
        private const int charImageBoxWidth = 860;
        /// <summary>
        /// The x position of the characger image
        /// </summary>
        private const int charImageX = 400;
        /// <summary>
        /// The sprite of the character image
        /// </summary>
        private CharSprite characterSprite;
        /// <summary>
        /// Accept button
        /// </summary>
        private ButtonSprite acceptButton;
        /// <summary>
        /// Cancel button
        /// </summary>
        private ButtonSprite cancelButton;
        /// <summary>
        /// Array of left arrow button
        /// </summary>
        private ArrowButtonSprite[] leftArrows;
        /// <summary>
        /// Array of right arrow button
        /// </summary>
        private ArrowButtonSprite[] rightArrows;
        /// <summary>
        /// The number of the body part
        /// </summary>
        private int bodyPartButtonNum  = 5;
        /// <summary>
        /// The x position of the buttons
        /// </summary>
        private int buttonX = 50;
        /// <summary>
        /// The y position of the topmost button
        /// </summary>
        private int buttonY = 100;
        /// <summary>
        /// Array of text of the body parts of the character
        /// </summary>
        private string [] bodyTexts = { "Head", "Face", "Body", "Legs", "Feet" };
        /// <summary>
        /// Gender of the character
        /// </summary>
        private Gender gender;
        /// <summary>
        /// Text of the gender of the character
        /// </summary>
        private TPString genderText; 
        /// <summary>
        /// Left arrow button for the gender
        /// </summary>
        private ArrowButtonSprite genderLeftButton;
        /// <summary>
        /// Right arrow button for the gender
        /// </summary>
        private ArrowButtonSprite genderRightButton;
        /// <summary>
        /// Textbox for user to insert a name for the character
        /// </summary>
        private General_TextBox nameTextBox;

        /// <summary>
        /// The colour of the character information
        /// </summary>
        private Color textColour = Color.Black;
        /// <summary>
        /// The colour of the titles
        /// </summary>
        private Color titleColour = Color.BlueViolet;

        /// <summary>
        /// Load and create everything that this state needs. 
        /// </summary>
        protected override void Load()
        {
            base.Load();

            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }

            //Character background boxes 
            TPSprite topBackgroundBox = new TPSprite(TPEngine.Get().TextureManager.CreateFilledRectangle(charBoxWidth, charBoxHeight, boxColor * 0.3f));
            topBackgroundBox.Position = new Vector2(charInfoX, 25);
            layer[1].AddEntity(topBackgroundBox);
            TPSprite botBackgroundBox = new TPSprite(TPEngine.Get().TextureManager.CreateFilledRectangle(charBoxWidth, bodyPartsBoxHeight, boxColor * 0.3f));
            botBackgroundBox.Position = new Vector2(charInfoX, 25 * 2 + charBoxHeight);
            layer[1].AddEntity(botBackgroundBox);
            TPSprite charImageBackgroundBox = new TPSprite(TPEngine.Get().TextureManager.CreateFilledRectangle(charImageBoxWidth, charImageBoxHeight, boxColor * 0.3f));
            charImageBackgroundBox.Position = new Vector2(charImageX, 25);
            layer[1].AddEntity(charImageBackgroundBox);

            //Character
            characterSprite = new CharSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/character_male_still"));
            characterSprite.Scale = new Vector2(2, 2);
            characterSprite.Position = new Vector2(charImageX + charImageBoxWidth / 2 - characterSprite.Width, 0);
            layer[1].AddEntity(characterSprite);

            //Name
            TPString nameTitle = new TPString("Name");
            nameTitle.Position = new Vector2(buttonX, buttonY - 50);
            nameTitle.RenderColor = titleColour;
            layer[1].AddEntity(nameTitle);

            nameTextBox = new General_TextBox(mousePointer, new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/rectangleTexture")), new Vector2(buttonX, buttonY), Color.White);
            layer[1].AddEntity(nameTextBox);

            //Gender
            gender = Gender.m;
            TPString genderTitle = new TPString("Gender");
            genderTitle.Position = new Vector2(buttonX, buttonY + 50);
            genderTitle.RenderColor = titleColour;

            genderLeftButton = new ArrowButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/LeftArrowOrange"),
                    new Rectangle(buttonX, buttonY + 100, 50, 50));
            genderRightButton = new ArrowButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RightArrowOrange"),
                    new Rectangle(buttonX + 200, buttonY + 100, 50, 50));
            genderText = new TPString("Male");
            genderText.Position = new Vector2(buttonX + genderLeftButton.Width * 9 / 5, buttonY + 100 + genderLeftButton.Height / 10);
            genderText.RenderColor = textColour;

            genderLeftButton.ButtonAction = switchGender;
            genderRightButton.ButtonAction = switchGender;

            layer[1].AddEntity(genderTitle);
            layer[1].AddEntity(genderText);
            layer[1].AddEntity(genderLeftButton);
            layer[1].AddEntity(genderRightButton);

            //Body parts buttons
            TPString bodyPartTitle = new TPString("Body Parts");
            bodyPartTitle.Position = new Vector2(buttonX, buttonY + 200);
            bodyPartTitle.RenderColor = titleColour;
            layer[1].AddEntity(bodyPartTitle);
            leftArrows = new ArrowButtonSprite[bodyPartButtonNum];
            rightArrows = new ArrowButtonSprite[bodyPartButtonNum];
            for (int i = 0; i < bodyPartButtonNum; i++)
            {
                leftArrows[i] = new ArrowButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/LeftArrowBlue"),
                    new Rectangle(buttonX, buttonY + 250 + i * 70, 50, 50));
                rightArrows[i] = new ArrowButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RightArrowBlue"),
                    new Rectangle(buttonX + 200, buttonY + 250 + i * 70, 50, 50));

                TPString bodyText = new TPString(bodyTexts[i]);
                bodyText.Position = new Vector2(buttonX + leftArrows[i].Width * 9 / 5, buttonY + 250 + i * 70 + leftArrows[i].Height / 10);
                bodyText.RenderColor = textColour;
                leftArrows[i].ButtonAction = delegate { TPEngine.Get().State.PushState(new ConfirmationPopup("Not implemented", true, false), true); };
                rightArrows[i].ButtonAction = delegate { TPEngine.Get().State.PushState(new ConfirmationPopup("Not implemented", true, false), true); };

                layer[1].AddEntity(leftArrows[i]);
                layer[1].AddEntity(rightArrows[i]);
                layer[1].AddEntity(bodyText);
            }

            //Accept and Cancel button
            acceptButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                new Rectangle(screenWidth / 2 - 100, screenHeight * 9 / 10, 200, 40), "Accept", layer[2]);
            cancelButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                new Rectangle(screenWidth / 2 + 300, screenHeight * 9 / 10, 200, 40), "Cancel", layer[2]);
            acceptButton.CenterText = true;
            cancelButton.CenterText = true;
            //Write the character data to the database via the web service
            acceptButton.ButtonAction = uploadCharacter;
            //Set the cancel button action to pop the current state
            cancelButton.ButtonAction = delegate { TPEngine.Get().State.PopState(); };

            layer[1].AddEntity(acceptButton);
            layer[1].AddEntity(cancelButton);

            //Background and mouse pointer
            layer[0].AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/NC_Background")));
            layer[3].AddEntity(mousePointer);
        }
        
        /// <summary>
        /// Switch the gender according to the gender text.
        /// </summary>
        private void switchGender()
        {
            if (gender == Gender.m)
            {
                gender = Gender.f;
                genderText.Clear();
                genderText.Append("Female");
                genderText.Position = new Vector2(buttonX + genderLeftButton.Width * 7 / 5, buttonY + 100 + genderLeftButton.Height / 10);
                characterSprite.setTexture(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/character_female_still"));
                //characterSprite.Scale = new Vector2(2, 2);
                //characterSprite.Position = new Vector2(charImageX + charImageBoxWidth / 2 - characterSprite.Width, 0);
            }
            else
            {
                gender = Gender.m;
                genderText.Clear();
                genderText.Append("Male");
                genderText.Position = new Vector2(buttonX + genderLeftButton.Width * 9 / 5, buttonY + 100 + genderLeftButton.Height / 10);
                characterSprite.setTexture(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/character_male_still"));
                //characterSprite.Scale = new Vector2(2, 2);
                //characterSprite.Position = new Vector2(charImageX + charImageBoxWidth / 2 - characterSprite.Width, 0);
            }
        }

        /// <summary>
        /// Write the new character data to the database via the web service
        /// </summary>
        private void uploadCharacter()
        {
            String nameText = nameTextBox.getinputString().Trim();
            //Check for empty name
            if (nameText == string.Empty)
            {
                TPEngine.Get().State.PushState(new ConfirmationPopup("Name cannot be empty", true, false), true);
                return;
            }
            //Check the length of the name
            else if (nameText.Length > 15)
            {
                TPEngine.Get().State.PushState(new ConfirmationPopup("Name cannot be longer" + 
                    Environment.NewLine + "than 15 characters", true, false), true);
                return;
            }
            //Check special characters in the name
            else if (!new Regex("^[a-zA-Z0-9 _]*$").IsMatch(nameText))
            {
                TPEngine.Get().State.PushState(new ConfirmationPopup("Name cannot contain" + 
                    Environment.NewLine + "special characters", true, false), true);
                return;
            }
            //Login
            User user = GameLauncher_LoginButtonSprite.getUser();
            DBCommServiceClient server = new DBCommServiceClient();
            Character character = new Character();
            character.name = nameText;
            character.sex = gender.ToString();
            character.position = "hall";
            character.money = 100;
            character.week = 1;
            character.hour = 7;
            server.SaveNewCharacterData(character, user);
            TPEngine.Get().State.PopState();
            TPEngine.Get().State.PopState();
            TPEngine.Get().State.PushState(new LoadCharacter());
        }
    }

    /// <summary>
    /// Extends the TpSprite class to allow switching textures
    /// </summary>
    class CharSprite : TPSprite
    {
        /// <summary>
        /// Constructor that calls the base constructor
        /// </summary>
        /// <param name="tex">Texture of the sprite</param>
        public CharSprite(Texture2D tex) : base(tex) { }

        /// <summary>
        /// Change the texture
        /// </summary>
        /// <param name="texture">Texture of the sprite</param>
        public void setTexture(Texture2D texture)
        {
            m_Texture = texture;
        }
    }
}
