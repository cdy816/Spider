//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/18 9:09:19.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    public static class Extends
    {
        public static void BackFile(this string sfile)
        {
            try
            {
                if (System.IO.File.Exists(sfile))
                {
                    string bfile = sfile + "_b";
                    if (System.IO.File.Exists(bfile))
                    {
                        System.IO.File.Delete(bfile);
                    }
                    System.IO.File.Copy(sfile, sfile + "_b");
                }
            }
            catch(Exception ex)
            {
                LoggerService.Service.Warn("BackFile",sfile+ " "+ ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="baseName"></param>
        /// <returns></returns>
        public static string GetAvaiableName(this ICollection<string> names,string baseName)
        {
            string sname = baseName;
            for (int i = 1; i < int.MaxValue; i++)
            {
                sname = baseName + i;
                if (!names.Contains(sname))
                {
                    return sname;
                }
            }
            return baseName;
        }
    }
}
