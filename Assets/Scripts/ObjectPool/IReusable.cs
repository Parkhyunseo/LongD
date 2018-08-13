using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


interface IReuseable
{
    void Initailize(); // 풀에서 나올 때 실행
    void Hibernate(); // 풀에 들어갈 때 실행
}