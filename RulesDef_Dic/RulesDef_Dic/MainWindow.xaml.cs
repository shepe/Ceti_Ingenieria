using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoFill();
        }

        private void AutoFill()
        {
            cmbPropList.Items.Clear();
            cmbResult.Items.Clear();
            cmbIPropAtm.Items.Clear();
            cmbOProp.Items.Clear();

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
                    cmbOProp.Items.Add(item.PropAtm);
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
                    lstRules.Items.Add(RebuiltRule(listProp, ConvertToString(item.Result), 'y'));
                }
            }
        }

        private string RebuiltRule(List<string> listProp, string p, char op)
        {
            if (listProp.Count > 0)
            {
                string finalRule = listProp.ElementAt(0);

                if (listProp.Count > 1)
                {
                    for (int i = 1; i < listProp.Count; i++)
                    {
                        finalRule = finalRule + " " + op + " " + listProp.ElementAt(i);
                    }
                }

                if (op == 'y')
                    finalRule = finalRule + " entonces " + p;
                else
                    finalRule = finalRule + " " + op + " " + p;

                return finalRule;
            }
            else
                return p;
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

        private int ConvertToInt(string s)
        {
            using(var db = new RulesModelContainer())
            {
                var query = from myvar in db.DictionarySet
                            orderby myvar.Id
                            where myvar.PropAtm == s
                            select myvar;

                foreach (var item in query)
                {
                    return item.Id;
                }

                return 0;
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
            txtPrevResult.Text = "";
            txtResult.Text = "";
        }

        private void btnRefresh2_Click(object sender, RoutedEventArgs e)
        {
            AutoFill();
        }

        //public List<string> listProp = new List<string>();
        public Boolean flag = false;
        public List<List<int>> listall = new List<List<int>>();

        public void fillRules()
        {
            using (var db = new RulesModelContainer())
            {
                var query = from myvar in db.RulesSet
                            orderby myvar.Id
                            select myvar;

                foreach (var item in query)
                {
                    List<int> list = new List<int>();
                    list.Clear();

                    foreach (var item2 in item.RulesDef)
                    {
                        item2.Prop = item2.Prop * -1;
                        list.Add(item2.Prop);
                    }

                    list.Add(item.Result);
                    listall.Add(list);
                }
            }
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            if (cmbIPropAtm.Text != "" && cmbIPropAtm.SelectedIndex != 0)
            {
                
                lstBefore.Items.Clear();
                lstAfter.Items.Clear();

                if (!flag)
                {
                    fillRules();
                    flag = true;
                }

                int value = 0;
                value = (chkNeg.IsChecked==true)?ConvertToInt(cmbIPropAtm.Text)*-1:ConvertToInt(cmbIPropAtm.Text);

                evalValue(value);
            }
            else
                MessageBox.Show("Primero seleccione lo que va a ingresar");
        }

        private void evalValue(int value)
        {
            List<string> listProp = new List<string>();
            List<List<int>> temp2 = new List<List<int>>();
            Boolean flagy = false;

            foreach (var item in listall)
            {
                listProp.Clear();
                foreach (var item2 in item)
                {
                    listProp.Add(ConvertToString(item2));
                    if (Math.Abs(item2) == Math.Abs(value))
                    {
                        if (item2 + value != 0)
                            flagy = true;
                    }
                }
                if(!flagy)
                    temp2.Add(item);
                    
                if (listProp.Count > 0)
                    lstBefore.Items.Add(RebuiltRule(listProp.GetRange(0, listProp.Count - 1), listProp.ElementAt(listProp.Count - 1), 'o'));

                flagy = false;
            }

            foreach (var item in temp2)
            {
                item.Remove(value*-1);
            }

            listall = temp2;

            foreach (var item in listall)
            {
                listProp.Clear();
                foreach (var item2 in item)
                {
                    listProp.Add(ConvertToString(item2));
                }

                if (listProp.Count > 0)
                    lstAfter.Items.Add(RebuiltRule(listProp.GetRange(0, listProp.Count - 1), listProp.ElementAt(listProp.Count - 1), 'o'));
            }

        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            lstAfter.Items.Clear();
            lstBefore.Items.Clear();
            listall.Clear();
        }

        private void btnInObj_Click(object sender, RoutedEventArgs e)
        {
            Tree mytree = new Tree(new Node(Math.Abs(ConvertToInt(cmbOProp.Text)), 1));
            string aux = "{ ";

            if (cmbOProp.Text != "" && cmbOProp.SelectedIndex != 0)
            {

                lstBefore.Items.Clear();
                lstAfter.Items.Clear();

                if (!flag)
                {
                    fillRules();
                    flag = true;
                }
                List<Node> l = new List<Node>();
                l.Add(mytree.root);
                mytree.review.Add(mytree.root.Value);
                mytree.GenTree(listall, l, mytree.review);

                String LD = "";

                foreach (var item in mytree.ContentTree)
                {
                    LD = ConvertToString(item.Value) + " -> ";
                    foreach (var item2 in item.LevelDependencies)
                    {
                        LD += ConvertToString(item2.Value) + " ";
                    }

                    lstTree.Items.Add(LD);
                    LD = "";
                }

                mytree.CutTree();

                foreach (var item in mytree.QueryList)
                {
                    foreach (var item2 in item.Split(' '))
                    {
                        try
                        {
                            aux += ConvertToString(int.Parse(item2)) + " ";
                        }
                        catch (Exception err) { }
                    }
                    aux += "}";
                    lstQuestionOrder.Items.Add(aux);
                    aux = "{ ";
                }
            }
            else
                MessageBox.Show("Primero seleccione lo que va a ingresar");
        }

        private void btnRestartObj_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            listall.Clear();
            lstTree.Items.Clear();
            lstQuestionOrder.Items.Clear();
        }

    }
}
