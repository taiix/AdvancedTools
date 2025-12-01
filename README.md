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

On Figure 4, we can see that the path nodes that are needed to create a path from A to B for agent 1 using A* are 35 nodes and it needs 9.148 seconds to reach the end goal. However, Figure 5 shows results for Theta* which using the same setting, the nodes that are visited are only 7 and the move time is 8.999 which is a bit faster than the path that A* creates. 

<div align="center">
  <figure>
    <img width="728" height="495" alt="image" src="https://github.com/user-attachments/assets/a2d8b029-1e92-4252-8233-f1660ae9891f" />
      <br>
        <figcaption><em>Figure 4:</em> Chart comparing path length across agents.</figcaption>
      </br>
  </figure>
</div>
<br>
<div align="center">
  <figure>
    <img width="728" height="495" alt="image" src="https://github.com/user-attachments/assets/c9eb5acc-625d-49f7-b8f5-987ac116ea5a" />
      <br>
        <figcaption><em>Figure 5:</em> Computation time results for A* vs Theta*.</figcaption>
      </br>
  </figure>
</div>
<br>

In the previous section, we conclude that Theta* uses less nodes and creates a bit shorter paths than A*. Next two figures, 6 and 7, are comparing the computation time of both algorithms. 
Let's consider the first agent that has the closest end goal and the last agent with the furthest end goal. From Figure 6 we know that agent 1 (using A*) has a path that goes through 140 nodes and the computation time that the algorithm needed to compute the path was 44 milliseconds. The same settings apply for the Theta* path, but in that case on Figure 7, the same route has only 17 nodes but the computation time is 63 milliseconds which is more than the A*.
The 4th agent(using A*), which is the one with the longest path has 549 nodes with computation time of 262 milliseconds, compared to Theta* with only 32 nodes but longer computation time of 341 milliseconds.

<br>

<div align="center">
  <figure>
    <img width="728" height="495" alt="image" src="https://github.com/user-attachments/assets/1c5068e0-25ce-4192-9c74-d80ab44a21af" />
       <br>
        <figcaption><em>Figure 6:</em> Computation time results for A*.</figcaption>
      </br>
  </figure>
</div>
<br>
<div align="center">
  <figure>
    <img width="728" height="495" alt="image" src="https://github.com/user-attachments/assets/63df25a7-b9fc-4a33-a7c7-6cfbe11d50a4" />
     <br>
        <figcaption><em>Figure 7:</em> Computation time results Theta*.</figcaption>
      </br>
  </figure>
</div>

### Conclusion
From all of the graphs and tests, we can conclude that based on the desired result, a different algorithm can be used. If we aim for optimization and faster results, A* is the best choise, but if a smoother, closer and more natural looking path is required, then Theta* is the better solution.


