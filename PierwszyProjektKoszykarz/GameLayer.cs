using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace PierwszyProjektKoszykarz
{
    public class GameLayer : CCLayerColor
    {

        // Define a label variable
        CCTextField t;
        CCSprite ludzik, siatka, siatka_brzeg, pilka;

        public GameLayer() : base(CCColor4B.Green)
        {
            //------//

            CCSprite bckg = new CCSprite("pole");
            bckg.Position = new CCPoint(0, 0);
            bckg.AnchorPoint = new CCPoint(0, 0);
            AddChild(bckg);

            ludzik = new CCSprite("ludz");
            ludzik.Position = new CCPoint(0, 0);
            ludzik.ScaleX = .7f;
            ludzik.ScaleY = .7f;
            AddChild(ludzik);

            siatka = new CCSprite("siatk");
            siatka.Position = new CCPoint(50, 600);
            siatka.ZOrder = 2;
            AddChild(siatka);

            siatka_brzeg = new CCSprite("siatka_brzeg");
            siatka_brzeg.Position = new CCPoint(50, 600);
            siatka_brzeg.Visible = false;
            AddChild(siatka_brzeg);


            pilka = new CCSprite("pilka");
            pilka.Position = new CCPoint(1000, 800);
            pilka.ScaleX = .5f;
            pilka.ScaleY = .5f;
            AddChild(pilka);


            t = new CCTextField("Punkty: 0", "Fonts/MarkerFelt", 22);
            t.AnchorPoint = CCPoint.AnchorUpperLeft;
            t.Position = new CCPoint(1000, 1000);
            t.Color = CCColor3B.Black;
            AddChild(t);


            Schedule(RUN);
            //------//
                    }


        float predkoscX, predkoscY;
        float grawitacja = 200;
        int punkty;
        public void RUN(float klatki)
        {

            predkoscY += klatki * -grawitacja;
            pilka.PositionX += predkoscX * klatki;
            pilka.PositionY += predkoscY * klatki;


            //Sprawdzenie zderzenia z koszykarzem

            bool CzyPilkaZderzaLudzika = pilka.BoundingBoxTransformedToParent.IntersectsRect(ludzik.BoundingBoxTransformedToParent);
            bool czyZmierzawDol = predkoscY < 0;

            if (CzyPilkaZderzaLudzika && czyZmierzawDol)
            {
                predkoscY *= -1f; //odbija                
                predkoscX = CCRandom.GetRandomFloat(-300, 300); //losowo odbija w lewo lub prawo
            }

            //Sprawdzenie zderzenia z krawedziami ekranu
             
            float pilkaPrawa = pilka.BoundingBoxTransformedToParent.MaxX; //prawa krawedz pilki
            float pilkaLewa = pilka.BoundingBoxTransformedToParent.MinX; //lewa krawedz pilki
           
            float ekranPrawa = VisibleBoundsWorldspace.MaxX;
            float ekranLewa = VisibleBoundsWorldspace.MinX;

                           
            if (pilkaPrawa > ekranPrawa && predkoscX > 0 || pilkaLewa < ekranLewa && predkoscX < 0)
            {
                predkoscX *= -1;
            }

            //Sprawdzenie czy wpad³a do kosza- sposob prosty, naiwny
            bool CzyPilkaZderzaSiatke = pilka.BoundingBoxTransformedToParent.IntersectsRect(siatka_brzeg.BoundingBoxTransformedToParent);
           
            if (CzyPilkaZderzaSiatke && czyZmierzawDol)
            {
                punkty++;
                t.Text = "Punkty: " + punkty;
                pilka.PositionX = 500;              
            }

            if (pilka.PositionY <0) pilka.PositionY = 1000;

        }



        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
          

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);


            touchListener.OnTouchesMoved = HandleTouchesMoved; //-----------//
           
        }

        //-----Obsluga dotkniecia ekranu -----//
        void HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            ludzik.PositionX = touches[0].Location.X; //0 bo pierwsze dotkniecie
            //ludzik.PositionY = touches[0].Location.Y;
        }
        //-----------//


        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }
    }
}

