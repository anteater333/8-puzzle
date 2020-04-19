8-puzzle problem
===========

## 부경대학교 컴퓨터공학과 인공지능 강의 과제#2

 * 과제 : A* 알고리즘을 구현하여 8-puzzle puzzle을 해결하자.  
 * 사용 언어 : C#  

## 1. 문제 파악
 
### A* 알고리즘

```
A* 알고리즘은 주어진 출발 꼭짓점에서부터 목표 꼭짓점까지 가는 최단 경로를 찾아내는 그래프/트리 탐색 알고리즘 중 하나이다. 이 알고리즘은 각 꼭짓점 x에 대해 그 꼭짓점을 통과하는 최상의 경로를 추정하는 순위값인 "휴리스틱 추정값" h(x)을 매기는 방법을 쓴다. 이 알고리즘은 이 휴리스틱 추정값의 순서로 꼭짓점을 방문한다.
```
(Ref. [위키백과 A* 알고리즘 문서](https://ko.wikipedia.org/wiki/A*_%EC%95%8C%EA%B3%A0%EB%A6%AC%EC%A6%98))

A* 알고리즘은 각 노드에서 목표 노드까지 도달하는데 필요한 비용을 추정한 값을 사용해 다음 경로를 선택하는 알고리즘이다. 최적 경로를 위한 선택은 평가 함수 f(n)를 통해 정해지고 이 평가 함수 f(n)는 다음 식으로 정의된다.
```
f(n) = g(n) + h(n)
g(n) : 현재까지의 비용
h(n) : 앞으로 예상되는 비용
```

### 8-puzzle에의 적용
![8-puzzle](./_img/8-puzzle_by_ddong.jpg)  
8-puzzle에서 노드는 퍼즐의 현재 상태가 된다. 현재 노드의 자식 노드는 현재 퍼즐의 상태에서 움직일 수 있는 새로운 경로가 된다. 평가 함수 f(n)은 다음과 같이 표현할 수 있다.  
```
f(n) = g(n) + h(n)
g(n) : 현재까지 이동한 횟수
h(n) : 퍼즐의 목표 상태와 현재 상태를 비교했을 때 서로 다른 칸의 개수
```

## 2. 구현
### class EPNode (EPNode.cs)  
8 퍼즐의 상태에 대한 정보를 저장하는 클래스.

Constructor :
 * `EPNode(int[,] matrix, int x, int y, int level, EPNode parent)`
    * `matrix` : 8 퍼즐의 상태를 나타내는 2차원 배열
    * `x` : 빈 칸의 x 좌표
    * `y` : 빈 칸의 y 좌표
    * `level` : 시작 노드로부터의 거리
    * `parent` : 부모 노드

Properties :
 * `Parent` : 부모 노드
 * `Distance` : 시작 노드로부터의 거리
 * `Heuristic` : 목표 노드까지의 추정치(초기값 = INF)
 * `Matrix` : 8 퍼즐의 상태를 나타내는 2차원 배열

APIs :
 * `EPNode MoveUp()`
 * `EPNode MoveDown()`
 * `EPNode MoveLeft()`
 * `EPNode MoveRight()` : 현재 노드의 퍼즐을 해당 방향으로 이동시킨 상태를 새 노드 인스턴스로 반환, 이동 불가 시 `null` 반환
 * `EPNode Estimate(int[,] goal)` : 목표 노드에 대한 현재 노드의 추정치 계산
 * `EPNode Print()` : 현재 노드 출력

! `Estimate`와 `Print()`의 반환 타입을 `EPNode`로 지정한 이유:  
![Test](./_img/test_01.PNG)  
`this`를 리턴하기 때문에 테스트 하기에 편합니다.

## class EightPuzzle (EightPuzzle.cs)
EPNode를 가지고 8 퍼즐 문제를 푸는 클래스. (EightPuzzle **has-a** EPNode)

Constructor :
 * `EightPuzzle(int[,] initial, int[,] goal, int limit)`
    * `initial` : 초기 상태
    * `goal` : 목표 상태
    * `limit` : 탐색 한도 (무한 루프를 방지, 유의미하게 큰 값)

API :
 * `bool Solve()` : A* 알고리즘을 사용해 8 퍼즐 문제 풀이 시작

## 3. 실행
### 코드

```
static void Main(string[] args)
{
    int[,] initial  = new int[,] { { 2, 8, 3 }, { 1, 6, 4 }, { 7, 0, 5 } };
    int[,] goal     = new int[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };
    int limit = 5000;

    Console.WriteLine("Solve the 8-Puzzle within " + limit + " search(es).");
    if ( new EightPuzzle(initial, goal, limit).Solve() )
    {
        Console.WriteLine("Solved!");
    }
    else
    {
        Console.WriteLine("Failed!");
    }
}
```

### 결과
![result](./_img/result.png)  

## 4. 알려진 문제
```
initial = { { 3, 8, 1 }, { 6, 2, 5 }, { 0, 4, 7 } }
goal    = { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } }
```
현재 코드는 사이클 판단과 백트래킹이 구현되어 있지 않기 때문에 위와 같은 입력에서 답을 찾지 못하고 다음과 같이 무한 루프에 빠진다.  
![failed](./_img/fail.png)  



#
anteater333@github