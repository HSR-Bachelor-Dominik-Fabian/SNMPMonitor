﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace SNMPManager.ServiceLayer
{
    [RunInstaller(true)]
    public partial class SNMPServiceInstaller : System.Configuration.Install.Installer
    {
        public SNMPServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
