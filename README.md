# Performance of A* and Theta* algorithms

# Introduction
In most of the games correct AI behavior is crutial. In this project, I evaluate and compare two pathfinding algorithms to find which one is better in this scenario.

# A*
A* is one of the most commonly used pathfinding algorithms. It gives you the shortest path from a point to a different point in the world by using value "f" which is equal to the sum of value "g" where "g" is the movement cost from the starting node to another node, and value "h" which is the movement cost from the current node to the end node.
In the upcoming section I will compare two different distance calculation, Manhattan distance and Euclidean distance.

### Manhattan distance 
Manhattan distance is the sum of absolute values of the difference in the coordinates between the current node and the last node : h = abs(currentNode.x - endNode.x) + abs(currentNode.y - endNode.y). This one is used when 4 directional movement is allowed.

### Euclidean distance 
Euclidean distance is the distance between the current node and the end node using the Pithagoras theorem.
h =  Mathf.Sqrt(Mathf.Pow((currentNode.x - endNode.x), 2) + Mathf.Pow((currentNode.y - endNode.y), 2)); and it's used to calculate distance where 8 directional movements are allowed.

### Difference between Manhattan and Euclidean
In my implementation, I use both distance calculation to compare them and see which one is better and faster in this situation.

<div align="center">
  <figure>
    <img width="1576" height="883" alt="image" src="https://github.com/user-attachments/assets/c5273890-e0c6-4095-a1b5-3b904eaf662e" />
      <figcaption><em>Figure 1:</em> <em>Note:</em> The snowmans are the agents. There are multiple agents, they just have the same position.</figcaption>
  </figure>
</div>
<br>

First, let's start with the Manhattan Distance. In Figure 2, we can see the number of nodes each agent traveled and the time required to reach the end node. Because Manhattan distance restrict movement to only four directions (up, down, left and right), agents often take more steps to reach the goal compared to Euclidean distance, which allows eight directions. This result paths that are longer, increases the node count and overall movement time. For example, the first agent needed to traverse 47 nodes to reach the goal, while with Euclidean distance (Figure 3) the same agent required only 35 nodes, with a difference in traversal time of nearly one second.






<br>
<div align="center">
  <figure>
    <img width="762" height="459" alt="image" src="https://github.com/user-attachments/assets/511679b7-f8ef-43ad-a241-11caecfb853e" />
     <br>
       <figcaption><em>Figure 2:</em> Manhattan Distance used by the agents, the amount of nodes the the agent has and the time spent to reach the end goal. </figcaption>
    </br>
  </figure>
</div>
<br>
<div align="center">
  <figure>
    <img width="762" height="459" alt="image" src="https://github.com/user-attachments/assets/fca615dd-f7f3-46bf-8a90-45b8d9de43a9" />
      <br>
        <figcaption><em>Figure 3:</em> Eucledian Distance used by the agents, the amount of nodes the the agent has and the time spent to reach the end goal.</figcaption>
      </br>
  </figure>
</div>


# Theta*

Theta* algorithm extends the A* algorithm that I used and makes it faster and smoother. The algorithm uses Line of Sight to determine if there is a straight line between two points. If the parent node can "see" the next node without obstacles, Theta* connects them directly. Althought they are similar and Theta* is built upon A*, they have some differences. They handle parent relationships differently. A* uses grid based approach where the parent must be a neightbour node, while Theta* allows any node that has a clear line of sight to be a parent. A new parent is only assigned when line of sight is blocked, which enables Theta* to generate shorter, more natural paths compared to the strictly grid based paths of A*.

<div align="center">
  <figure>
    <img width="728" height="495" alt="image" src="https://github.com/user-attachments/assets/a2d8b029-1e92-4252-8233-f1660ae9891f" />
      <br>
        <figcaption><em>Figure 3:</em> Chart comparing path length across agents.</figcaption>
      </br>
  </figure>
</div>

<div align="center">
  <figure>
    <img width="728" height="495" alt="image" src="https://github.com/user-attachments/assets/c9eb5acc-625d-49f7-b8f5-987ac116ea5a" />
      <br>
        <figcaption><em>Figure 4:</em> Computation time results for A* vs Theta*.</figcaption>
      </br>
  </figure>
</div>

<img width="914" height="546" alt="image" src="https://github.com/user-attachments/assets/63df25a7-b9fc-4a33-a7c7-6cfbe11d50a4" />
<img width="914" height="546" alt="image" src="https://github.com/user-attachments/assets/1c5068e0-25ce-4192-9c74-d80ab44a21af" />


