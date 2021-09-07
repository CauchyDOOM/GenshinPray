﻿using GenshinPray.Attribute;
using GenshinPray.Exceptions;
using GenshinPray.Models;
using GenshinPray.Models.PO;
using GenshinPray.Service;
using GenshinPray.Type;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GenshinPray.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RolePrayController : BasePrayController<RolePrayService>
    {
        /// <summary>
        /// 单抽角色祈愿池
        /// </summary>
        /// <param name="memberCode">玩家编号(可以传入QQ号)</param>
        /// <param name="toBase64"></param>
        /// <param name="imgWidth"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode]
        public override ApiResult PrayOne(string memberCode, bool toBase64 = false, int imgWidth = 0)
        {
            try
            {
                int prayCount = 1;
                checkImgWidth(imgWidth);

                var authorzation = HttpContext.Request.Headers["authorzation"];
                AuthorizePO authorizePO = authorizeService.GetAuthorize(authorzation);

                int prayTimesToday = prayRecordService.GetPrayTimesToday(authorizePO.Id);
                if (prayTimesToday >= authorizePO.DailyCall) return ApiResult.ApiMaximum;

                MemberPO memberInfo = memberService.GetOrInsert(authorizePO.Id, memberCode);
                YSUpItem ySUpItem = goodsService.GetUpItem(authorizePO.Id, YSPondType.角色);
                YSPrayResult ySPrayResult = basePrayService.GetPrayResult(memberInfo, ySUpItem, prayCount, imgWidth);

                memberInfo.RolePrayTimes += prayCount;
                memberService.UpdateMemberInfo(memberInfo);//更新保底信息
                prayRecordService.AddPrayRecord(authorizePO.Id, memberCode, prayCount);//添加调用记录
                memberGoodsService.AddMemberGoods(ySPrayResult, authorizePO.Id, memberCode);//添加成员出货记录

                ApiPrayResult prayResult = basePrayService.CreatePrayResult(ySUpItem, ySPrayResult, authorizePO, prayTimesToday, toBase64);
                prayResult.Surplus180 = memberInfo.Role180Surplus;
                prayResult.Surplus90 = memberInfo.Role90Surplus;
                return ApiResult.Success(prayResult);
            }
            catch (BaseException ex)
            {
                return ApiResult.Error(ex);
            }
            catch (Exception ex)
            {
                //LogHelper.LogError(ex);
                return ApiResult.ServerError;
            }
        }

        /// <summary>
        /// 十连角色祈愿池
        /// </summary>
        /// <param name="memberCode">玩家编号(可以传入QQ号)</param>
        /// <param name="toBase64"></param>
        /// <param name="imgWidth"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode]
        public override ApiResult PrayTen(string memberCode, bool toBase64 = false, int imgWidth = 0)
        {
            try
            {
                int prayCount = 10;
                checkImgWidth(imgWidth);

                var authorzation = HttpContext.Request.Headers["authorzation"];
                AuthorizePO authorizePO = authorizeService.GetAuthorize(authorzation);

                int prayTimesToday = prayRecordService.GetPrayTimesToday(authorizePO.Id);
                if (prayTimesToday >= authorizePO.DailyCall) return ApiResult.ApiMaximum;

                MemberPO memberInfo = memberService.GetOrInsert(authorizePO.Id, memberCode);
                YSUpItem ySUpItem = goodsService.GetUpItem(authorizePO.Id, YSPondType.角色);
                YSPrayResult ySPrayResult = basePrayService.GetPrayResult(memberInfo, ySUpItem, prayCount, imgWidth);

                memberInfo.RolePrayTimes += prayCount;
                memberService.UpdateMemberInfo(memberInfo);//更新保底信息
                prayRecordService.AddPrayRecord(authorizePO.Id, memberCode, prayCount);//添加调用记录
                memberGoodsService.AddMemberGoods(ySPrayResult, authorizePO.Id, memberCode);//添加成员出货记录

                ApiPrayResult prayResult = basePrayService.CreatePrayResult(ySUpItem, ySPrayResult, authorizePO, prayTimesToday, toBase64);
                prayResult.Surplus180 = memberInfo.Role180Surplus;
                prayResult.Surplus90 = memberInfo.Role90Surplus;
                return ApiResult.Success(prayResult);
            }
            catch (BaseException ex)
            {
                return ApiResult.Error(ex);
            }
            catch (Exception ex)
            {
                //LogHelper.LogError(ex);
                return ApiResult.ServerError;
            }
        }





    }

}
