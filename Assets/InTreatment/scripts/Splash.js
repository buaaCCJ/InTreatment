//-----------------------------------------------------------------------------------------
// Simple script to show Splash image before game
//-----------------------------------------------------------------------------------------
#pragma strict
 
 
 var splashTexture: GUITexture;	// Splash image texture
 var appearanceTime:float = 1;	// How fast should it appear/disappear
 var stayTime:float = 2;		// How long should splash be wisible before disappearing
 
 // Important internal variables - please don't change them blindly
 private var appearanceAtTime:float;
 private var stayAtTime:float;
 private var state = -1;
 

//=======================================================================================================	
// Initialize internal variables
function Start () 
{
      Time.timeScale = 1;
	  splashTexture.pixelInset.width = Screen.width;
	  splashTexture.pixelInset.height = Screen.height;
	  splashTexture.color.a = 0;
  
           
      appearanceAtTime = Time.time + appearanceTime;
      state = 0;
    
    if (!splashTexture.texture)  Application.LoadLevel (Application.loadedLevel+1);
}

//----------------------------------------------------------------------------------
// Process Splash through all stages
function Update () 
{

     // Process appearance animation
  	  if (state == 0)
	     if (Time.time > appearanceAtTime) 
	       {
	         state = 1;
	         stayAtTime = Time.time + stayTime;
	       }
	       else
	         splashTexture.color.a += Time.deltaTime;
    
     // Stay on screen
      if (state == 1) 
      	   if (Time.time > stayAtTime) 
	       {
	         state = 2;
	         appearanceAtTime = Time.time + appearanceTime;
	       }

	 // Process disappearance animation
      if (state == 2)
	     if (Time.time > appearanceAtTime) 
	       {
	        state = 3;
	       }
	       else
	         splashTexture.color.a -= Time.deltaTime;


  
	 // Load next level     
	    if (state == 3 ) Application.LoadLevel (Application.loadedLevel+1);
     
   
}

//----------------------------------------------------------------------------------