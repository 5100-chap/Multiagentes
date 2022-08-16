void A_star(){
  for(int n = 0; n < solvIter; n++){
    if(openSet.size() > 0){
      int winner = 0;
      for (int i = 0; i < openSet.size(); i++){
        if(openSet.get(i).f < openSet.get(winner).f){
          winner = i;
        }
      }
      
      current = openSet.get(winner);
      openSet.remove(current);
      closedSet.add(current);
      for(Spot neighbor : current.neighbors){
        if(neighbor.wall == false && !closedSet.contains(neighbor)){
          //float tempG = neighbor.g = current.g + 1;
          float tempG = neighbor.g = current.g + heuristic(neighbor, current);
          if(openSet.contains(neighbor)){//really need? replace by neighbor.g == 0?
            if (tempG < neighbor.g){
              neighbor.g = tempG;
            }
          }
          else{//first visit
            neighbor.g = tempG;
            openSet.add(neighbor);
          }
          neighbor.h = heuristic(neighbor, end);
          neighbor.calcF();
          neighbor.previous = current;
        }
      }
      if(current == end){
        println("DONE");
        solving = false;
        break;
      }
    }else{
      println("NO SOLUTION");
      solving = false;
      break;
    }
  }
}