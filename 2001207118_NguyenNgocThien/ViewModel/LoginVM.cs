﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace _2001207118_NguyenNgocThien.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username cannot be blank.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password cannot be blank.")]
        public string Password { get; set; }
    }
}