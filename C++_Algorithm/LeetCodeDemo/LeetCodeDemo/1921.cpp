using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//�������� ģ��
//f=��������ڼ����ӿ�ʼ���յ�
class Solution {
public:
    int eliminateMaximum(vector<int>& dist, vector<int>& speed) {
        const int MAXN = 1e5;
        int n = dist.size();
        int f[MAXN]{};

        for (int i = 0; i < n; i++) {
            f[i] = dist[i] % speed[i]?dist[i] / speed[i]+1: dist[i] / speed[i];
        }
        sort(f, f + n);
        for (int i = 0; i < n; i++) {
            if (f[i] <= i) {
                return i;
            }
        }
        return n;
    }
};