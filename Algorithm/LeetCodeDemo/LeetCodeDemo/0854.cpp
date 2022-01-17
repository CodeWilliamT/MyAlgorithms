 using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
#include <queue>
 //广搜
 class Solution {
 public:
     int kSimilarity(string A, string B) {
         // 如果相等则直接返回
         if (A == B)
         {
             return 0;
         }

         int n = A.size();

         // pair.first[0] 之前操作的次数(currUsed，默认为0) + 后续要预计最小操作的次数 (currExp)
         // pair.first[1] 之前操作的次数 (currUsed)
         // 这里优先级队列就是按照 小顶堆，就是总的操作次数越小的在队列上方
         auto cmp = [](const pair<vector<int>, string>& l, const pair<vector<int>, string>& r)
         {
             return l.first[0] > r.first[0];
         };
         priority_queue<pair<vector<int>, string>, vector<pair<vector<int>, string>>, decltype(cmp)> open(cmp);

         // 构成字符串 到 最小操作次数 映射
         unordered_map<string, int> str2minUsed;

         // 插入初始化A
         int currUsed = 0;
         int currExp = MiniOperations(A, B);
         open.push({ {currUsed + currExp, currUsed}, A });
         str2minUsed[A] = 0;
         while (!open.empty()) {
             auto top = open.top();
             open.pop();
             auto currStr = top.second;
             auto currUsed = top.first[1];
             // 如果相等，直接返回即可
             if (currStr == B)
             {
                 return currUsed;
             }
             int i = 0;
             // 找到下一个和B不相等元素的编号
             for (; i < n && currStr[i] == B[i]; ++i);
             // 遍历得到相等的j, 并且交换
             for (int j = i + 1; j < n; ++j)
             {
                 // A的编号j和B的i相等
                 if (currStr[j] == B[i])
                 {
                     // 做一次交换，让A[i]=B[i]
                     string nextStr = currStr;
                     swap(nextStr[i], nextStr[j]);
                     // 这里做了一次交换，所以+1
                     int nextUsed = currUsed + 1;
                     int nextExp = nextUsed + MiniOperations(nextStr, B);
                     // 如果为空或者找到更小操作的次数，则更新
                     if (str2minUsed.find(nextStr) == str2minUsed.end() || str2minUsed[nextStr] > nextUsed)
                     {
                         str2minUsed[nextStr] = nextUsed;
                         open.push({ {nextExp, nextUsed}, nextStr });
                     }
                 }
             }
         }
         return 0;
     }

     // 最小交换次数
     int MiniOperations(const string& s, const string& end)
     {
         int sum = 0;
         for (int i = 0; i < s.size(); i++) {
             sum += s[i] != end[i];
         }
         return (sum + 1) / 2;
     }
 };