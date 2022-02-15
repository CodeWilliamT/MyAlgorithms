using namespace std;
#include <vector>
//找规律 巧思
//遍历矩阵，找出行最小的列索引，列最大的行索引；
//遍历行,行最小的列索引指向的行索引等于当前行，则符合条件
class Solution {
public:
    vector<int> luckyNumbers(vector<vector<int>>& matrix) {
        int m = matrix.size(),n = matrix[0].size();
        int col[50]{}, row[50]{};
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if(matrix[i][j]<matrix[i][row[i]])
                    row[i] = j;
                if (matrix[i][j] > matrix[col[j]][j])
                    col[j]= i;
            }
        }
        vector<int> rst;
        for (int i = 0; i < m; i++) {
            if (col[row[i]] ==i) {
                rst.push_back(matrix[i][row[i]]);
            }
        }
        return rst;
    }
};