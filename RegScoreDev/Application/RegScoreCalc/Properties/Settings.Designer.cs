﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegScoreCalc.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\Test_data.mdb;P" +
            "ersist Security Info=True")]
        public string Test_dataConnectionString {
            get {
                return ((string)(this["Test_dataConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Resources\\trainTemp" +
            "late.accdb;Persist Security Info=True")]
        public string trainTemplateConnectionString {
            get {
                return ((string)(this["trainTemplateConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ProcessName {
            get {
                return ((string)(this["ProcessName"]));
            }
            set {
                this["ProcessName"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Test_data_Billing.md" +
            "b")]
        public string Test_data_BillingConnectionString {
            get {
                return ((string)(this["Test_data_BillingConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\Test_data_Billi" +
            "ng.mdb;Persist Security Info=True")]
        public string Test_data_BillingConnectionString1 {
            get {
                return ((string)(this["Test_data_BillingConnectionString1"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<xs:schema attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""DocumentElement"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Documents"" maxOccurs=""unbounded"" minOccurs=""0"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ED_ENC_NUM"" type=""xs:double"" />
                <xs:element name=""NOTE_TEXT"" minOccurs=""0"">
                  <xs:simpleType>
                    <xs:restriction base=""xs:string"">
                      <xs:maxLength value=""536870910"" />
                    </xs:restriction>
                  </xs:simpleType>
                </xs:element>
              <xs:element type=""xs:string"" name=""Category""/>
              <xs:element type=""xs:int"" name=""Score""/>
              <!-- ADD HERE DYNAMIC -->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>")]
        public string XMLSchemaTemplate {
            get {
                return ((string)(this["XMLSchemaTemplate"]));
            }
            set {
                this["XMLSchemaTemplate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FormRegExpEditorShowExamples {
            get {
                return ((bool)(this["FormRegExpEditorShowExamples"]));
            }
            set {
                this["FormRegExpEditorShowExamples"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FormRegExpEditorShowQuickActions {
            get {
                return ((bool)(this["FormRegExpEditorShowQuickActions"]));
            }
            set {
                this["FormRegExpEditorShowQuickActions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormRegExpEditorToolboxPaneWidth {
            get {
                return ((int)(this["FormRegExpEditorToolboxPaneWidth"]));
            }
            set {
                this["FormRegExpEditorToolboxPaneWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormRegExpEditorExamplesPaneHeight {
            get {
                return ((int)(this["FormRegExpEditorExamplesPaneHeight"]));
            }
            set {
                this["FormRegExpEditorExamplesPaneHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormAutoAssignCattegoryScoreCondition {
            get {
                return ((int)(this["FormAutoAssignCattegoryScoreCondition"]));
            }
            set {
                this["FormAutoAssignCattegoryScoreCondition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormAutoAssignCattegoryScoreValue {
            get {
                return ((int)(this["FormAutoAssignCattegoryScoreValue"]));
            }
            set {
                this["FormAutoAssignCattegoryScoreValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormEditLookaround_Width {
            get {
                return ((int)(this["FormEditLookaround_Width"]));
            }
            set {
                this["FormEditLookaround_Width"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormEditLookaround_Height {
            get {
                return ((int)(this["FormEditLookaround_Height"]));
            }
            set {
                this["FormEditLookaround_Height"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FormEditLookaround_SplitterDistance {
            get {
                return ((int)(this["FormEditLookaround_SplitterDistance"]));
            }
            set {
                this["FormEditLookaround_SplitterDistance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1, -1")]
        public global::System.Drawing.Point FormSelectCategory_Location {
            get {
                return ((global::System.Drawing.Point)(this["FormSelectCategory_Location"]));
            }
            set {
                this["FormSelectCategory_Location"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1, -1")]
        public global::System.Drawing.Size FormSelectCategory_Size {
            get {
                return ((global::System.Drawing.Size)(this["FormSelectCategory_Size"]));
            }
            set {
                this["FormSelectCategory_Size"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\\WORK\\Nisimvw\\RegScoreCalc3\\RegSco" +
            "reCalc-Ding\\RegScoreDev-branch\\Bootstrapper\\Data\\Test.mdb;Persist Security Info=" +
            "True")]
        public string TestConnectionString {
            get {
                return ((string)(this["TestConnectionString"]));
            }
        }
    }
}