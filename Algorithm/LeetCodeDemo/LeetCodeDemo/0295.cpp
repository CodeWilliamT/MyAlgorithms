using namespace std;
#include <iostream>
#include <set>
//利用multiset，以及迭代器。
//在每次插入时进行对中值进行更新，将其与插入值进行比较。保持Mid的位置为0.5count(偶数时为靠后那个，奇数为正中间那个);
//输出答案奇数为*mid或偶数为(*mid+*prev(mid))*0.5
class MedianFinder {
public:
    /** initialize your data structure here. */
    multiset<int> data; 
    multiset<int>::iterator mid;
    int count;
    MedianFinder() {
        mid = data.end();
        count = 0;
    }

    void addNum(int num) {
        data.insert(num);
        if (!count)//当集合原本为空时
            mid = data.begin();
        else if (num < *mid)//插入值小于中位数
            mid = (count & 1 ? mid : prev(mid));//集合原本是奇数，则不变，是偶数，则变为mid的前一个。
        else//插入值大等于中位数
            mid = (count & 1 ? next(mid) : mid);//集合原本是奇数，则变为mid的后一个，是偶数，则不变
        count++;
    }

    double findMedian() {
        return count%2?*mid:(*mid + *prev(mid)) * 0.5;
    }
};
//int main()
//{
//    MedianFinder s;
//    s.addNum(0);
//    s.addNum(0);
//    s.addNum(0);
//    s.addNum(1);
//    s.addNum(2);
//    s.addNum(3);
//    s.addNum(5);
//    s.addNum(6);
//    s.addNum(6);
//    s.addNum(6);
//    s.addNum(10);
//    cout << s.findMedian() << endl;
//
//    return 0;
//}