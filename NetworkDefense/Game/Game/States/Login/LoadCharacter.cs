using System.Text;
using Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Engine.Objects;
using System;
using Game.Sprites;
using DBCommService;
using Game.Quests;

//Ting Fung (Kevin) Ng
//Wing Lim (William) Cheng

namespace Game.States
{
    /// <summary>
    /// Load the character of the current user if there is one existed. If not, 
    /// creates a create-character-button that leads the user to the New Character state.
    /// </summary>
    class LoadCharacter : TPState
    {
        /// <summary>
        /// The layer array that holds everything.
        /// </summary>
        private TPLayer[] layer;
        /// <summary>
        /// The number of layers
        /// </summary>
        private Int16 NumLayers = 5;
        /// <summary>
        /// The mouse pointer object
        /// </summary>
        private MousePointerSprite mousePointer = new MousePointerSprite(new Vector2(32, 32));
        /// <summary>
        /// The array that holds buttons
        /// </summary>
        private ButtonSprite[] buttonArray;
        /// <summary>
        /// The number of buttons
        /// </summary>
        private int buttonNum = 3;
        /// <summary>
        /// The array of strings of the buttons
        /// </summary>
        private string[] buttonText = { "Load", "Delete", "Cancel" };
        /// <summary>
        /// The x position of the button
        /// </summary>
        private int buttonX = 240;
        /// <summary>
        /// The y position of the button
        /// </summary>
        private int buttonY = 650;
        /// <summary>
        /// The button object for creating character
        /// </summary>
        private ButtonSprite createCharButton;
        /// <summary>
        /// The height for the character box
        /// </summary>
        private const int charBoxHeight = 400;
        /// <summary>
        /// The width for the character box
        /// </summary>
        private const int charBoxWidth = 800;
        /// <summary>
        /// The color of the character box background
        /// </summary>
        private Color boxColor = Color.SteelBlue;
        /// <summary>
        /// The x position of the character info box
        /// </summary>
        private const int charInfoX = 240;
        /// <summary>
        /// The height of the character info box
        /// </summary>
        private const int textBoxHeight = 150;
        /// <summary>
        /// The array of strings for the character info
        /// </summary>
        private TPString[] charInfo;
        /// <summary>
        /// The color of the strings for the character info
        /// </summary>
        private Color infoTextColor = Color.Black;
        /// <summary>
        /// The sprite of the character
        /// </summary>
        private TPSprite characterSprite;
        /// <summary>
        /// The user object for logging in
        /// </summary>
        private User user;
        /// <summary>
        /// The server object for logging in
        /// </summary>
        private DBCommServiceClient server;
        /// <summary>
        /// The character object from the database
        /// </summary>
        private Character character;
        /// <summary>
        /// The result of the confirmation popup
        /// </summary>
        private ConfirmationResult confirmResult;
        /// <summary>
        /// The state for the loading screen
        /// </summary>
        private TPState loadscreen;
        /// <summary>
        /// If the state is currently loading or not
        /// </summary>
        private bool loading = false;

        /// <summary>
        /// Constructor that initializes a confirmation result.
        /// </summary>
        public LoadCharacter()
        {
            confirmResult = new ConfirmationResult();
        }

        /// <summary>
        /// Load and create everything this state needs
        /// </summary>
        protected override void Load()
        {
            base.Load();
            //Create layers
            layer = new TPLayer[NumLayers];
            for (int x = 0; x < NumLayers; x++)
            {
                layer[x] = new TPLayer(this.layers);
            }

            //Loading
            loadscreen = new Loading();
            TPEngine.Get().State.PushState(loadscreen);
            TPEngine.Get().GameRef.Tick();

            //Login
            user = GameLauncher_LoginButtonSprite.getUser();
            server = new DBCommServiceClient();
            character = server.LoadCharacterData(user);

            //Quit loading
            TPEngine.Get().State.PopState();

            //Character background boxes 
            TPSprite topBackgroundBox = new TPSprite(TPEngine.Get().TextureManager.CreateFilledRectangle(charBoxWidth, charBoxHeight, boxColor * 0.5f));
            topBackgroundBox.Position = new Vector2(charInfoX, 25);
            layer[1].AddEntity(topBackgroundBox);
            TPSprite botBackgroundBox = new TPSprite(TPEngine.Get().TextureManager.CreateFilledRectangle(charBoxWidth, textBoxHeight, boxColor * 0.5f));
            botBackgroundBox.Position = new Vector2(charInfoX, 25 * 2 + charBoxHeight);
            layer[1].AddEntity(botBackgroundBox);

            //Character
            if (character == null)
            {
                createNewCharacter();
            }
            else
            {
                //Character
                if(character.sex == "m")
                    characterSprite = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/character_male_still"));
                else
                    characterSprite = new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/character_female_still"));
                characterSprite.Scale = new Vector2(1.5f, 1.5f);
                characterSprite.Position = new Vector2(charInfoX + charBoxWidth / 2 - characterSprite.Width / 1.5f, -50);
                layer[1].AddEntity(characterSprite);

                //Character Information
                charInfo = new TPString[4];
                charInfo[0] = new TPString("Name: " + character.name);
                charInfo[1] = new TPString("Grade: " + character.grades);
                charInfo[2] = new TPString("Money: " + character.money);
                charInfo[3] = new TPString("Score: " + character.global_score);

                for (int i = 0; i < 2; i++)
                {
                    charInfo[i].Position = new Vector2(charInfoX + 25 + i * charBoxWidth / 2, charBoxHeight + 50 + textBoxHeight / 10);
                    charInfo[i].RenderColor = infoTextColor;
                    layer[3].AddEntity(charInfo[i]);
                }
                for (int i = 2; i < 4; i++)
                {
                    charInfo[i].Position = new Vector2(charInfoX + 25 + (i - 2) * charBoxWidth / 2, charBoxHeight + 50 + textBoxHeight / 2);
                    charInfo[i].RenderColor = infoTextColor;
                    layer[3].AddEntity(charInfo[i]);
                }
            }

            //GetQuests() must come before SignalNewQuestDay().
            QuestChecking.GetQuests();

            AreaState.character = character;

            if (character != null)
            {
                //Uses AreaState.character so do not call if character is null. Must come after AreaState.character = character.
                QuestChecking.SignalNewQuestDay();
            }

            //Buttons
            buttonArray = new ButtonSprite[buttonNum];
            for (int i = 0; i < buttonNum; i++)
            {
                buttonArray[i] = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                    TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                    new Rectangle(buttonX + i * 300, buttonY, 200, 40), buttonText[i], layer[3]);
                buttonArray[i].CenterText = true;
                layer[2].AddEntity(buttonArray[i]);
            }
            //Set the load button action to push the game state
            buttonArray[0].ButtonAction = delegate { loading = true; TPEngine.Get().GameRef.Tick(); TPEngine.Get().State.PushState(new AreaState(character.position)); };
            //Set the delete button action to delete the current character
            buttonArray[1].ButtonAction = delegate
            {
                TPEngine.Get().State.PushState(new ConfirmationPopup("Are you sure you want to delete?", confirmResult), true);
            };
            //Set the cancel button action to pop the current state
            buttonArray[2].ButtonAction = delegate { TPEngine.Get().State.PopState(); };

            //Disable the delete button when the character does not exist.
            if (character == null)
                buttonArray[1].Disabled = true;

            layer[0].AddEntity(new TPSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/NC_Background")));
            layer[3].AddEntity(mousePointer);
        }

        /// <summary>
        /// Create a button that brings the user to the New Character state.
        /// </summary>
        public void createNewCharacter()
        {
            //Create character button
            createCharButton = new ButtonSprite(TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectBlackBlueBurst"),
                TPEngine.Get().TextureManager.LoadTexture(@"art/MenuStates/RectOrangeBlueBurst"),
                new Rectangle(charInfoX + 25, 50, charBoxWidth - 50, charBoxHeight - 50), "Create Character", layer[3]);
            createCharButton.CenterText = true;
            createCharButton.ButtonAction = delegate { TPEngine.Get().State.PushState(new NewCharacter(), true); };
            layer[2].AddEntity(createCharButton);

            //No character text
            TPString noCharacterText = new TPString("No existing character");
            noCharacterText.Position = new Vector2(charInfoX + charBoxWidth / 2, 25 * 2 + charBoxHeight + textBoxHeight / 2);
            noCharacterText.centerText = true;
            noCharacterText.RenderColor = Color.White * 0.5f;
            layer[3].AddEntity(noCharacterText);
        }

        /// <summary>
        /// Update the state.
        /// </summary>
        /// <param name="gameTime">Time of the game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Delete the character if the user has confirmed to delete it
            //After that, load a new Load Character state
            if (confirmResult.result)
            {
                buttonArray[1].Disabled = false;
                server.DeleteCharacter(user);
                TPEngine.Get().State.PopState();
                TPEngine.Get().State.PushState(new LoadCharacter());
                confirmResult.result = false;
            }
        }

        /// <summary>
        /// Draw the loading screen if it is loading
        /// </summary>
        /// <param name="batch">SpriteBatch for drawing</param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            if (loading)
                loadscreen.Draw(batch);
        }
    }
}