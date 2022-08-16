float heuristic(Spot a, Spot b){
  switch(heurType){
    case 0:
      return (a.i-b.i) * (a.i-b.i) + (a.j-b.j) * (a.j-b.j);
    case 1:
      return dist(a.i, a.j, b.i, b.j);
    case 2:
      return abs(a.i-b.i) + abs(a.j-b.j);
    default:
      return 1;
  }
}

void betWallBreak(Spot a, Spot b){
  a.wall = false;
  b.wall = false;
  Spot mid = grid[(a.i+b.i)/2][(a.j+b.j)/2];
  mid.wall = false;
  mid.visited = true;
}

void reset(){
  mazeReset();
  routeReset();
}

void routeReset(){
  solving = true;
  openSet = new ArrayList<Spot>();
  closedSet = new ArrayList<Spot>();
  path = new ArrayList<Spot>();
  start = grid[1][1];
  openSet.add(start);
}

void mazeReset(){
  mazeBuilding = true;
  for(int i = 0; i < cols; i++){
    for(int j = 0; j < rows; j++){
      grid[i][j] = new Spot(i, j);
    }
  }
  
  for(int i = 0; i < cols; i++){
    for(int j = 0; j < rows; j++){
      grid[i][j].addNeighbors();
    }
  }
  start = grid[1][1];
  start.wall = false;
  end = grid[cols-1-cols%2][rows-1-rows%2];
  end.wall = false;
  current = null;
  genCurrent = start;
  genStack.add(genCurrent);
}

void showAll(){
  fill(360);
  stroke(0);
  strokeWeight(1);
  noStroke();
  for(int i = 0; i < cols; i++){
    for(int j = 0; j < rows; j++){
      Spot cell = grid[i][j];
      cell.show(color(360));
    }
  }
  
  for(Spot spot : closedSet){
    color col = color(0);
    switch(debugMode){
      case 0:
        col = color(spot.f*1%360, 100, 100, 50);
        break;
      case 1:
        col = color(spot.g*1%360, 100, 100, 50);
        break;
      default:
        col = color(spot.h*1%360, 100, 100, 50);
        break;
    }
    spot.show(col);
  }
  if(mazeBuilding){
    genCurrent.show(color(100, 100, 100));
  }
  else{
    for(Spot spot : openSet){
      spot.show(color(100, 100, 100));
    }
  }
  
  noFill();
  strokeWeight(wid/2);
  stroke(300, 100, 100, 100);
  path = new ArrayList<Spot>();
  Spot temp = current;
  if(temp != null){
    path.add(temp);
    while(temp.previous != null){
      path.add(temp.previous);
      temp = temp.previous;
    }
    beginShape();
    for(Spot seg : path){
      vertex(seg.i*wid+wid/2, seg.j*hei+hei/2);
      //seg.show(color(200, 100, 100));
    }
    endShape();
  }
}