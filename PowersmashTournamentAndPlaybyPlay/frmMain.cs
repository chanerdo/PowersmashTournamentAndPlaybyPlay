using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace PowersmashTournamentAndPlaybyPlay
{
    public partial class frmMain : Form
    {

        private MySqlConnection connDB = new MySqlConnection("datasource=unicsoftworks.com;port=3306;username=admin;password=powersmash123;Convert Zero Datetime=true;");
        private DataTable tournament_data;
        private DataTable user_data;
        private DataTable reservation_data;
        private DataTable team_data;
        private DataTable user_group_data;
        private int sp1, sp2, dp1, dp2, dp3, dp4, mp1, mp2, mp3, mp4;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            getTournamentData();
            getUserData();
            getReservationData();
            getTeamData();
            getUserGroupData();
            
            foreach (DataRow tour_row in tournament_data.Rows)
            {
                cbxTournamentName.Items.Add(tour_row.Field<string>(1));
            }
            foreach (DataRow reserve_row in reservation_data.Rows)
            {
                if (reserve_row.Field<int>(8) == 3)
                {
                    cbxCourtTour.Items.Add(reserve_row.Field<int>(2));
                }
            }
            foreach (DataRow team_row in team_data.Rows)
            {
                cbxSingleTeam1.Items.Add(team_row.Field<string>(1));
                cbxSingleTeam2.Items.Add(team_row.Field<string>(1));
                cbxDoubleTeam1.Items.Add(team_row.Field<string>(1));
                cbxDoubleTeam2.Items.Add(team_row.Field<string>(1));
                cbxMixTeam1.Items.Add(team_row.Field<string>(1));
                cbxMixTeam2.Items.Add(team_row.Field<string>(1));
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if(tournament_data.Rows.Count != 0)
            {
                cbxTournamentName.SelectedIndex = 0;
            }
            if (reservation_data.Rows.Count != 0)
            {
                cbxCourtTour.SelectedIndex = 0;
            }
            cbxMatchType.SelectedIndex = 0;
            cbxGender.SelectedIndex = 0;
            cbxSingleTeam1.SelectedIndex = 0;
            cbxSingleTeam2.SelectedIndex = 0;
            cbxDoubleTeam1.SelectedIndex = 0;
            cbxDoubleTeam2.SelectedIndex = 0;
            cbxMixTeam1.SelectedIndex = 0;
            cbxMixTeam2.SelectedIndex = 0;
        }

        private void getTournamentData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.tournament", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                tournament_data = new DataTable();
                sqlDataAdapter.Fill(tournament_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getUserData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.user", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                user_data = new DataTable();
                sqlDataAdapter.Fill(user_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getReservationData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.reservation", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                reservation_data = new DataTable();
                sqlDataAdapter.Fill(reservation_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getTeamData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.team", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                team_data = new DataTable();
                sqlDataAdapter.Fill(team_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getUserGroupData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.fos_user_user_group", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                user_group_data = new DataTable();
                sqlDataAdapter.Fill(user_group_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void saveData(string query)
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand(query, connDB);
                cmdDB.ExecuteNonQuery();
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            finally
            {
                connDB.Close();
            }
        }

        private void cbxMatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMatchType.SelectedIndex == 0)
            {
                gbxSingle.Show();
                gbxDouble.Hide();
                gbxMix.Hide();
            }
            if (cbxMatchType.SelectedIndex == 1)
            {
                gbxDouble.Show();
                gbxSingle.Hide();
                gbxMix.Hide();
            }
            if (cbxMatchType.SelectedIndex == 2)
            {
                gbxMix.Show();
                gbxSingle.Hide();
                gbxDouble.Hide();
            }
        }

        private void cbxGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*for (int i = 0; i < dgvSinglePlayer.RowCount; i++)
            {
                dgvSinglePlayer.Rows[i].Visible = true ;
            }
            dgvSinglePlayer.Invalidate();

            if (cbxMatchType.SelectedIndex == 0)
            {
                for (int i = 0; i < dgvSinglePlayer.RowCount; i++)
                {
                    if (cbxGender.SelectedIndex == 0)
                    {
                        if (!dgvSinglePlayer.Rows[i].Cells["gender"].Value.ToString().Equals("m"))
                        {
                            dgvSinglePlayer.CurrentCell = null;
                            dgvSinglePlayer.Rows[i].Visible = false;
                            dgvSinglePlayer.Invalidate();
                        }
                    }
                    if (cbxGender.SelectedIndex == 1)
                    {
                        if (!dgvSinglePlayer.Rows[i].Cells["gender"].Value.ToString().Equals("f"))
                        {
                            dgvSinglePlayer.CurrentCell = null;
                            dgvSinglePlayer.Rows[i].Visible = false;
                            dgvSinglePlayer.Invalidate();
                        }
                    }
                }
            }*/
        }

        private void cbxMixTeam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlayerMixName1.Items.Count != 0)
            {
                for (int i = 0; i < cbxPlayerDoubleName1.Items.Count; i++)
                {
                    cbxPlayerMixName1.Items.RemoveAt(i);
                    cbxPlayerMixName2.Items.RemoveAt(i);
                }
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxMixTeam1.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerMixName1.Items.Add(name);
                                        cbxPlayerMixName2.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxMixTeam1.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerMixName1.Items.Add(name);
                                        cbxPlayerMixName2.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            cbxPlayerMixName1.SelectedIndex = 0;
            cbxPlayerMixName2.SelectedIndex = 0;
        }

        private void cbxMixTeam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlayerMixName3.Items.Count != 0)
            {
                for (int i = 0; i < cbxPlayerMixName3.Items.Count; i++)
                {
                    cbxPlayerMixName3.Items.RemoveAt(i);
                    cbxPlayerMixName4.Items.RemoveAt(i);
                }
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxMixTeam2.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerMixName3.Items.Add(name);
                                        cbxPlayerMixName4.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxMixTeam2.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerMixName3.Items.Add(name);
                                        cbxPlayerMixName4.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            cbxPlayerMixName3.SelectedIndex = 0;
            cbxPlayerMixName4.SelectedIndex = 0;
        }

        private void cbxPlayerMixName1_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerMixName1.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        mp1 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMixPlayer1.Image = Image.FromStream(memstream);
                        mp1 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxPlayerMixName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerMixName2.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        mp2 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMixPlayer2.Image = Image.FromStream(memstream);
                        mp2 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxPlayerMixName3_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerMixName3.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        mp3 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMixPlayer3.Image = Image.FromStream(memstream);
                        mp3 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxPlayerMixName4_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerMixName4.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        mp4 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMixPlayer4.Image = Image.FromStream(memstream);
                        mp4 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void btnMixDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you done?", "Done", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string matchgame = "INSERT INTO powersmash.match (player11, player12, player21, player22, tournament_id, court_id, type) VALUES((SELECT id FROM powersmash.user WHERE id = '" + mp1 +
                                   "'), (SELECT id FROM powersmash.user WHERE id = '" + mp2 + "'), (SELECT id FROM powersmash.user WHERE id = '" + mp3 + "'), (SELECT id FROM powersmash.user WHERE id = '" + mp4 +
                                   "'), (SELECT id FROM powersmash.tournament WHERE name = '" + cbxTournamentName.Text + "'), '" + cbxCourtTour.Text + "', 'Mixed')";
                saveData(matchgame);

                frmPlaybyPlay play = new frmPlaybyPlay(cbxTournamentName.Text, cbxMixTeam1.Text, cbxMixTeam2.Text, cbxPlayerMixName1.Text, cbxPlayerMixName2.Text, cbxPlayerMixName3.Text, cbxPlayerMixName4.Text, 3, mp1, mp2, mp3, mp4);
                play.Show();
                this.Hide();
            }
        }

        private void cbxDoubleTeam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlayerDoubleName1.Items.Count != 0)
            {
                for (int i = 0; i < cbxPlayerDoubleName1.Items.Count; i++)
                {
                    cbxPlayerDoubleName1.Items.RemoveAt(i);
                    cbxPlayerDoubleName2.Items.RemoveAt(i);
                }
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxDoubleTeam1.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerDoubleName1.Items.Add(name);
                                        cbxPlayerDoubleName2.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxDoubleTeam1.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerDoubleName1.Items.Add(name);
                                        cbxPlayerDoubleName2.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            cbxPlayerDoubleName1.SelectedIndex = 0;
            cbxPlayerDoubleName2.SelectedIndex = 0;
        }

        private void cbxDoubleTeam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlayerDoubleName3.Items.Count != 0)
            {
                for (int i = 0; i < cbxPlayerDoubleName3.Items.Count; i++)
                {
                    cbxPlayerDoubleName3.Items.RemoveAt(i);
                    cbxPlayerDoubleName4.Items.RemoveAt(i);
                }
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxDoubleTeam2.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerDoubleName3.Items.Add(name);
                                        cbxPlayerDoubleName4.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxDoubleTeam2.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerDoubleName3.Items.Add(name);
                                        cbxPlayerDoubleName4.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            cbxPlayerDoubleName3.SelectedIndex = 0;
            cbxPlayerDoubleName4.SelectedIndex = 0;
        }

        private void cbxDoublePlayerName1_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerDoubleName1.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        dp1 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxDoublePlayer1.Image = Image.FromStream(memstream);
                        dp1 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxDoublePlayerName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerDoubleName2.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        dp2 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxDoublePlayer2.Image = Image.FromStream(memstream);
                        dp2 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxDoublePlayerName3_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerDoubleName3.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        dp3 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxDoublePlayer3.Image = Image.FromStream(memstream);
                        dp3 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxDoublePlayerName4_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerDoubleName4.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals(""))
                    {
                        dp4 = user_row.Field<int>(0);
                    }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxDoublePlayer4.Image = Image.FromStream(memstream);
                        dp4 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void btnDoubleDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you done?", "Done", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string matchgame = "INSERT INTO powersmash.match (player11, player12, player21, player22, tournament_id, court_id, type) VALUES((SELECT id FROM powersmash.user WHERE id = '" + dp1 +
                                   "'), (SELECT id FROM powersmash.user WHERE id = '" + dp2 + "'), (SELECT id FROM powersmash.user WHERE id = '" + dp3 + "'), (SELECT id FROM powersmash.user WHERE id = '" + dp4 + 
                                   "'), (SELECT id FROM powersmash.tournament WHERE name = '" + cbxTournamentName.Text + "'), '" + cbxCourtTour.Text + "', 'Doubles')";
                saveData(matchgame);

                frmPlaybyPlay play = new frmPlaybyPlay(cbxTournamentName.Text, cbxDoubleTeam1.Text, cbxDoubleTeam2.Text, cbxPlayerDoubleName1.Text, cbxPlayerDoubleName2.Text, cbxPlayerDoubleName3.Text, cbxPlayerDoubleName4.Text, 2, dp1, dp2, dp3, dp4);
                play.Show();
                this.Hide();
            }
        }

        private void cbxSingleTeam1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlayerSingleName1.Items.Count != 0)
            {
                for (int i = 0; i < cbxPlayerSingleName1.Items.Count; i++)
                {
                    cbxPlayerSingleName1.Items.RemoveAt(i);
                }
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxSingleTeam1.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerSingleName1.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxSingleTeam1.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerSingleName1.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            cbxPlayerSingleName1.SelectedIndex = 0;
        }

        private void cbxSingleTeam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPlayerSingleName1.Items.Count != 0)
            {
                for (int i = 0; i < cbxPlayerSingleName2.Items.Count; i++)
                {
                    cbxPlayerSingleName2.Items.RemoveAt(i);
                }
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxSingleTeam2.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerSingleName2.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow team_row in team_data.Rows)
                {
                    if (team_row.Field<string>(1).Equals(cbxSingleTeam2.Text))
                    {
                        foreach (DataRow usergroup_row in user_group_data.Rows)
                        {
                            if (usergroup_row.Field<int>(1) == team_row.Field<int>(0))
                            {
                                foreach (DataRow user_row in user_data.Rows)
                                {
                                    if (user_row.Field<int>(0) == usergroup_row.Field<int>(0))
                                    {
                                        string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                                        cbxPlayerSingleName2.Items.Add(name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            cbxPlayerSingleName2.SelectedIndex = 0;
        }

        private void cbxPlayerSingleName1_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerSingleName1.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals("")) { sp1 = user_row.Field<int>(0); }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxSinglePlayer1.Image = Image.FromStream(memstream);
                        sp1 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void cbxPlayerSingleName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] image;
            int rowcount = 0;
            foreach (DataRow user_row in user_data.Rows)
            {
                string name = user_row.Field<string>(21) + " " + user_row.Field<string>(22);
                if (name.Equals(cbxPlayerSingleName2.Text))
                {
                    if (user_data.Rows[rowcount][42].ToString().Equals("")) { sp2 = user_row.Field<int>(0); }
                    else
                    {
                        image = (byte[])(user_row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxSinglePlayer2.Image = Image.FromStream(memstream);
                        sp2 = user_row.Field<int>(0);
                    }
                }
                rowcount++;
            }
        }

        private void btnSingleDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you done?", "Done", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show(sp1.ToString() + "\n" + sp2.ToString());
                string matchgame = "INSERT INTO powersmash.match (player11, player21, tournament_id, court_id, type) VALUES((SELECT id FROM powersmash.user WHERE id = '" + sp1 +
                                   "'), (SELECT id FROM powersmash.user WHERE id = '" + sp2 + "'), (SELECT id FROM powersmash.tournament WHERE name = '" + cbxTournamentName.Text +
                                   "'), '" + cbxCourtTour.Text + "', 'Singles')";
                saveData(matchgame);

                frmPlaybyPlay play = new frmPlaybyPlay(cbxTournamentName.Text, cbxSingleTeam1.Text, cbxSingleTeam2.Text, cbxPlayerSingleName1.Text, cbxPlayerSingleName2.Text, "", "", 1, sp1, sp2, 0, 0);
                play.Show();
                this.Hide();
            }
        }
    }
}
