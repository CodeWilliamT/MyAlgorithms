//简单题
//喝光，然后空瓶换酒再喝光，可用空瓶=喝光产生的空瓶+没换的空瓶，不够换了就完事了。
class Solution {
public:
    int numWaterBottles(int numBottles, int numExchange) {
        int rst = numBottles;
        while (numBottles / numExchange)
        {
            rst += numBottles / numExchange;
            numBottles = numBottles / numExchange + numBottles % numExchange;
        }
        return rst;
    }
};