using namespace std;
#include <vector>
//���� ����ģ�� O(64e6)
//������ͼ��ͨ��Ԫ���������Сֵ
//������ͨ��Ԫ�飬���Ҷ�
//��ͨ��Ԫ�飺���ݱ߼�������λͼ���������㣬�����ĵ㣬�����ĺ���㣺����������������λͼO(1)���ж����򹹳���Ԫ�顣
//��Ԫ��Ķȣ���������ܶ�-6��(ͳ�Ƹ�����Ķ�)
//��С���Ǵ𰸡�
class Solution {
#define MAXN 401
public:
    int minTrioDegree(int n, vector<vector<int>>& edges) {
        bool g[MAXN][MAXN]{};
        int lk[MAXN]{};
        for (auto& e : edges) {
            if (e[0] > e[1])swap(e[0], e[1]);
            g[e[0]][e[1]] = 1;
            lk[e[0]]++;
            lk[e[1]]++;
        }
        int rst = INT32_MAX;
        for (int i = 1; i <=n; i++) {
            for (int j = i+1; j <= n; j++) {
                for (int k = j + 1; k <= n; k++) {
                    if (g[i][j] && g[i][k] && g[j][k]) {
                        rst = min(lk[i] + lk[j] + lk[k] - 6, rst);
                    }
                }
            }
        }
        return rst== INT32_MAX?-1:rst;
    }
};