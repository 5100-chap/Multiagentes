void keyPressed(){
  if(key=='r'){
    routeReset();
  }
  if(key=='b'){
    reset();
  }
  if(key=='s'){
    heurType++;
    if(heurType >= 4){
      heurType = 0;
    }
  }
  if(key=='d'){
    debugMode++;
    if(debugMode >= 3){
      debugMode = 0;
    }
  }
}