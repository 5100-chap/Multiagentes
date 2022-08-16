
int cols = 60;
int rows = 60;
float wallRate=0.3;

Spot[][] grid = new Spot[cols][rows];
ArrayList<Spot> closedSet = new ArrayList<Spot>();
ArrayList<Spot> openSet = new ArrayList<Spot>();
ArrayList<Spot> path = new ArrayList<Spot>();
ArrayList<Spot> genStack = new ArrayList<Spot>();
float wid, hei;
int heurType = 0;
int debugMode = 0;
int genIter = 20;
int solvIter = 2;
boolean mazeBuilding = true;
boolean solving = true;

Spot genCurrent;
Spot current;
Spot start;
Spot end;

PImage bg;

void setup(){
  size(600, 600);
  //fullScreen();
  colorMode(HSB, 360, 100, 100, 100);
  bg = loadImage("img.jpg");
  wid = width/cols;
  hei = height/rows;
  reset();
  //frameRate(5);
}

void draw(){
  background(0);
  int iter = solvIter;
  if(mazeBuilding){
    iter = genIter;
  }
  
    if(mazeBuilding){
      mazeGen();
    }else if(solving){
      A_star();
    }
  
  
  showAll();
}
