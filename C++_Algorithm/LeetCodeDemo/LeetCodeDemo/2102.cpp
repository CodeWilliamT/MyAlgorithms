using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//设计题 优先队列
//思路：构建优先队列，头部队列pq1,降序，尾部队列pq2,升序.
//输入：元素为-积分，名字，存入pq1,然后拿pq1的队头(因为取反，所以输出的是pq1里评分最小的)，存入pq2，pq1.pop();
//查询：输出队尾的队头(因为取反，所以输出的是pq2里评分最大的)，然后放入头部队列pq1.
//输出的元素进已输出队列pq2,然后已输出队列的队头被挤出加入待输出队列，成为队头。
class SORTracker {
    priority_queue<pair<int, string>, vector<pair<int, string>>, less<pair<int, string>>> pq1;
    priority_queue<pair<int, string>, vector<pair<int, string>>, greater<pair<int, string>>> pq2;
public:
    SORTracker() {
        
    }

    void add(string name, int score) {
        pair<int, string> element= { -score,name };
        pq1.push(element);
        pq2.push(pq1.top());
        pq1.pop();
    }

    string get() {
        string val = pq2.top().second;
        pq1.push(pq2.top());
        pq2.pop();
        return val;
    }
};
