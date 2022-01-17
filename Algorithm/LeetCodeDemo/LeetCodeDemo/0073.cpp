using namespace std;
#include <iostream>
#include <unordered_set>
#include <vector>
#include <string>
//对于列，使用一个标记变量记录第一列是否原本存在 0。遍历时若j列有0，则更新第一行j列数据为0，最后决定第一行是否全置0
//对于行，为了防止每一列的第一个元素被提前更新，我们需要从最后一行开始，根据该元素值与第一行元素值是否为0，处理矩阵元素是否置为0。

class Solution {
public:
    void setZeroes(vector<vector<int>>& matrix) {
        int m = matrix.size();
        int n = matrix[0].size();
        int flag_col0 = false;
        for (int i = 0; i < m; i++) {
            if (!matrix[i][0]) {
                flag_col0 = true;
            }
            for (int j = 1; j < n; j++) {
                if (!matrix[i][j]) {
                    matrix[i][0] = matrix[0][j] = 0;
                }
            }
        }
        for (int i = m - 1; i >= 0; i--) {
            for (int j = 1; j < n; j++) {
                if (!matrix[i][0] || !matrix[0][j]) {
                    matrix[i][j] = 0;
                }
            }
            if (flag_col0) {
                matrix[i][0] = 0;
            }
        }
    }
};
////basic
//class Solution {
//public:
//    void setZeroes(vector<vector<int>>& matrix) {
//        int m = matrix.size();
//        if (m == 0)return;
//        int n = matrix[0].size();
//        if (n == 0)return;
//        unordered_set<int> a, b;
//        for (int i = 0; i < m; i++)
//        {
//            for (int j = 0; j < n; j++)
//            {
//                if (matrix[i][j] == 0)
//                {
//                    a.insert(i);
//                    b.insert(j);
//                }
//            }
//        }
//        for (auto cur : a)
//        {
//            for (int i = 0; i < n; i++)
//            {
//                matrix[cur][i] = 0;
//            }
//        }
//        for (auto cur : b)
//        {
//            for (int i = 0; i < m; i++)
//            {
//                matrix[i][cur] = 0;
//            }
//        }
//    }
//};
//int main()
//{
//    Solution s;
//    vector<vector<int>> matrix = { {0, 1, 2, 0}, {3, 4, 5, 2}, { 1, 3, 1, 5} };
//    s.setZeroes(matrix);
//    for (auto r : matrix)
//    {
//        for (auto c : r)
//            cout << to_string(c) + " ";
//        cout << endl;
//    }
//
//    return 0;
//}