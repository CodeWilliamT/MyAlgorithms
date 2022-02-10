using namespace std;
#include <vector>
#include <string>
//巧思 辗转相余法
class Solution {
    int gcd(int a,int b){
        return b==0?a:gcd(b,a%b);
    }
public:
    vector<string> simplifiedFractions(int n) {
        vector<string> rst;
        for(int i=2;i<=n;i++){
            for(int j=1;j<i;j++){
                if(gcd(i,j)!=1&&j!=1)continue;
                rst.push_back(to_string(j)+"/"+to_string(i));
            }
        }
        return rst;
    }
};