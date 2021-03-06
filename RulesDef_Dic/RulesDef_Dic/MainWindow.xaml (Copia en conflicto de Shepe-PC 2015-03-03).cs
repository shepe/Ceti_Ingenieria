﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RulesDef_Dic
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtProp.Text != "")
            {
                lstProp.Items.Add(txtProp.Text);
            }
            else
                MessageBox.Show("La proposicion atomica no es valida");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstProp.SelectedItem != null)
            {
                if (MessageBox.Show("Se ELIMINARA la proposicion seleccionada esta seguro de realizar esta accion",
                    "Eliminar Proposicion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    lstProp.Items.Remove(lstProp.SelectedItem);
                    lstProp.Items.Refresh();
                }
            }
            else
                MessageBox.Show("No ha seleccionado la proposicion a eliminar");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new RulesModelContainer())
            {
                foreach (var item in lstProp.Items)
                {
                    db.DictionarySet.Add(new Dictionary() { PropAtm = item.ToString() });
                }
                db.SaveChanges();
            }
        }

        private void btnShowActuals_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new RulesModelContainer())
            {
                var query = from myvar in db.DictionarySet
                            orderby myvar.Id
                            select myvar;

                lstProp2.Items.Clear();

                foreach (var item in query)
                {
                    lstProp2.Items.Add(item.PropAtm);
                }
            }
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Esta a punto de ELIMINAR todas las proposiciones atomicas de la base de datos, ¿Desea Continuar?", "Eliminar Todo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var db = new RulesModelContainer())
                {
                    var query = from myvar in db.DictionarySet
                                orderby myvar.Id
                                select myvar;
                    foreach (var item in query)
                    {
                        db.DictionarySet.Remove(item);
                    }
                    db.SaveChanges();
                }
            }
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    AutoFill();
        //}

        private void AutoFill()
        {
            cmbPropList.Items.Clear();
            cmbResult.Items.Clear();
            cmbIPropAtm.Items.Clear();

            using(var db = new RulesModelContainer())
            {
                var query = from myvar in db.DictionarySet
                            orderby myvar.Id
                            select myvar;

                foreach (var item in query)
                {
                    cmbPropList.Items.Add(item.PropAtm);
                    cmbResult.Items.Add(item.PropAtm);
                    cmbIPropAtm.Items.Add(item.PropAtm);
                }
            }
        }

        private void btnDeleteProp_Click(object sender, RoutedEventArgs e)
        {
            if (lstProp3.SelectedItem != null)
            {
                if (MessageBox.Show("Se ELIMINARA la proposicion seleccionada esta seguro de realizar esta accion",
                    "Eliminar Proposicion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    lstProp3.Items.Remove(lstProp3.SelectedItem);
                    lstProp3.Items.Refresh();
                    UpdateRuleView(lstProp3, txtResult);
                }
            }
            else
                MessageBox.Show("No ha seleccionado la proposicion a eliminar");
        }

        private void btnAddProp_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPropList.Text != "" && cmbPropList.SelectedIndex != 0)
            {
                if (cbNo.IsChecked == true)
                    lstProp3.Items.Add("-" + cmbPropList.Text);
                else
                    lstProp3.Items.Add(cmbPropList.Text);
                UpdateRuleView(lstProp3, txtResult);
            }
            else
                MessageBox.Show("No hay una proposicion seleccionada");
        }

        private void btnAddResult_Click(object sender, RoutedEventArgs e)
        {
            if (cmbResult.Text != "" && cmbResult.SelectedIndex != 0)
            {
                if (cbNoResult.IsChecked == true)
                    txtResult.Text = "-" + cmbResult.Text;
                else
                    txtResult.Text = cmbResult.Text;
                UpdateRuleView(lstProp3, txtResult);
            }
            else
                MessageBox.Show("No hay una conclusion seleccionada");
        }

        private void UpdateRuleView(ListBox list, TextBox textbox)
        {
            txtPrevResult.Clear();

            txtPrevResult.Text = ((list.Items.GetItemAt(0).ToString().ElementAt<char>(0) == '-') ? "NO " + list.Items.GetItemAt(0).ToString().Substring(1) : list.Items.GetItemAt(0).ToString());

            if (list.Items.Count > 1)
            {
                for (int i = 1; i < list.Items.Count; i++)
                {
                    txtPrevResult.Text = txtPrevResult.Text + " y " +
                                        ((list.Items.GetItemAt(i).ToString().ElementAt<char>(0) == '-') ? "NO " + list.Items.GetItemAt(i).ToString().Substring(1) : list.Items.GetItemAt(i).ToString());
                }
            }
            if (txtResult.Text != "")
            {
                txtPrevResult.Text = txtPrevResult.Text + " entonces " + ((txtResult.Text.ElementAt<char>(0) == '-') ? "NO " + txtResult.Text.Substring(1) : txtResult.Text);
            }
        }

        private void btnCreateRule_Click(object sender, RoutedEventArgs e)
        {
            if (lstProp3.Items.Count > 0 && txtResult.Text != "")
            {
                using (var db = new RulesModelContainer())
                {
                    if (ConvertToNumber(ReturnToPropAtm(txtResult.Text), NegValue(txtResult.Text)) != 0)
                    {
                        Rules rule = new Rules();
                        rule.Result = ConvertToNumber(ReturnToPropAtm(txtResult.Text), NegValue(txtResult.Text));
                        db.RulesSet.Add(rule);
                        foreach (var item in lstProp3.Items)
                        {
                            RulesDef r = new RulesDef();
                            r.Prop = ConvertToNumber(ReturnToPropAtm(item.ToString()), NegValue(item.ToString()));
                            db.RulesDefSet.Add(r);
                        }

                        db.SaveChanges();
                    }
                    else
                        MessageBox.Show("La conclusion no es valida");
                }
            }
            else
                MessageBox.Show("La regla es invalida");
        }

        private string ReturnToPropAtm(string s)
        {
            return ((s.ElementAt<char>(0) == '-')? s.Substring(1): s);
        }

        private bool NegValue(string s)
        {
            return ((s.ElementAt<char>(0) == '-') ? true : false);
        }

        private int ConvertToNumber(string s, bool n)
        {
            using(var db = new RulesModelContainer())
            {
                var query = from myvar in db.DictionarySet
                            orderby myvar.Id
                            where myvar.PropAtm == s
                            select myvar;

                foreach (var item in query)
                {
                    if (n == true)
                        return item.Id * (-1);
                    else
                        return item.Id;
                }

                return 0;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            AutoFill();
        }

        private void btnClearList1_Click(object sender, RoutedEventArgs e)
        {
            lstProp.Items.Clear();
        }

        private void btnShowRules_Click(object sender, RoutedEventArgs e)
        {
            lstRules.Items.Clear();
            List<string> listProp = new List<string>();

            using (var db = new RulesModelContainer())
            {
                var query = from rule in db.RulesSet
                            orderby rule.Id
                            select rule;

                foreach (var item in query)
                {
                    listProp.Clear();

                    var query2 = from r in db.RulesDefSet
                                 orderby r.Id
                                 where r.RulesId == item.Id
                                 select r;

                    foreach (var item2 in query2)
                    {
                        listProp.Add(ConvertToString(item2.Prop));
                    }
                    lstRules.Items.Add(RebuiltRule(listProp, ConvertToString(item.Result)));
                }
            }
        }

        private string RebuiltRule(List<string> listProp, string p)
        {
            string finalRule = listProp.ElementAt(0);

            if(listProp.Count > 1)
            {
                for(int i = 1; i < listProp.Count; i++)
                {
                    finalRule = finalRule + " y " + listProp.ElementAt(i);
                }
            }

            finalRule = finalRule + " entonces " + p;

            return finalRule;

        }

        private string ConvertToString(int p)
        {
            using(var db = new RulesModelContainer())
            {
                var query = from myvar in db.DictionarySet
                            orderby myvar.Id
                            where myvar.Id == Math.Abs(p)
                            select myvar;

                foreach (var item in query)
                {
                    return ((p < 0)? "NO " + item.PropAtm : item.PropAtm);
                }

                return "";
            }
        }

        private void btnBigDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Esta a punto de ELIMINAR todas las reglas de la base de datos, ¿Desea Continuar?", "Eliminar Todo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var db = new RulesModelContainer())
                {
                    var query = from myvar in db.RulesSet
                                orderby myvar.Id
                                select myvar;

                    foreach (var item in query)
                    {
                        db.RulesSet.Remove(item);
                    }

                    var query2 = from m in db.RulesDefSet
                                 orderby m.Id
                                 select m;

                    foreach (var item2 in query2)
                    {
                        db.RulesDefSet.Remove(item2);
                    }

                    db.SaveChanges();
                }
            }
        }

        private void btnClearLst2_Click(object sender, RoutedEventArgs e)
        {
            lstProp3.Items.Clear();
        }
    }
}
