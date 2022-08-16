class Spot{
  int i;
  int j;
  float f;
  float g = 0;
  float h = 0;
  ArrayList<Spot> neighbors = new ArrayList<Spot>();
  ArrayList<Spot> genNeighbors = new ArrayList<Spot>();
  Spot previous;
  boolean visited =false;
  boolean wall = true;
  
  Spot(int i, int j){
    this.i = i;
    this.j = j;
    previous = null;
    visited = false;
  }
  
  void calcF(){
    f = g + h;
  }
  
  void addNeighbors(){
    if(i < cols-1){
      neighbors.add(grid[i+1][j]);
    }
    if(i > 0){
      neighbors.add(grid[i-1][j]);
    }
    if(j < rows-1){
      neighbors.add(grid[i][j+1]);
    }
    if(j > 0){
      neighbors.add(grid[i][j-1]);
    }
  }
  
  Spot getNeighbor2(){
    genNeighbors = new ArrayList<Spot>();
    if(i < cols-2 && grid[i+2][j].visited == false){
      genNeighbors.add(grid[i+2][j]);
    }
    if(i > 1 && grid[i-2][j].visited == false){
      genNeighbors.add(grid[i-2][j]);
    }
    if(j < rows-2 && grid[i][j+2].visited == false){
      genNeighbors.add(grid[i][j+2]);
    }
    if(j > 1 && grid[i][j-2].visited == false){
      genNeighbors.add(grid[i][j-2]);
    }
    if(genNeighbors.size() > 0){
      int r = floor(random(0, genNeighbors.size()));
      return genNeighbors.get(r);
    }
    return null;
  }
  
  void show(color col){
    if(wall == true){
      fill(0);
    }else{
      fill(col);
    }
    rect(i*wid, j*hei, wid, hei);
  }
}