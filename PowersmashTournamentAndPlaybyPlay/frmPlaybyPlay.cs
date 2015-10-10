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
    public partial class frmPlaybyPlay : Form
    {

        private MySqlConnection connDB = new MySqlConnection("datasource=unicsoftworks.com;port=3306;username=admin;password=powersmash123;Convert Zero Datetime=true;");
        private DataTable user_data;
        private DataTable match_data;
        private Rectangle rect;
        private Image shuttle;
        private int player_id1, player_id2, player_id3, player_id4, matchtype, server, t1Score = 0, t2Score = 0, play_id = 0, team1_counter, team2_counter;
        private string tournamentname, team1, team2, player_name1, player_name2, player_name3, player_name4;

        public frmPlaybyPlay(string tourname, string t1, string t2, string pn1, string pn2, string pn3, string pn4, int mt, int pi1, int pi2, int pi3, int pi4)
        {
            matchtype = mt;
            if(matchtype == 2 || matchtype == 3)
            {
                tournamentname = tourname;
                team1 = t1;
                team2 = t2;
                player_name1 = pn1;
                player_name2 = pn2;
                player_name3 = pn3;
                player_name4 = pn4;
                player_id1 = pi1;
                player_id2 = pi2;
                player_id3 = pi3;
                player_id4 = pi4;
            }
            else if (matchtype == 1)
            {
                tournamentname = tourname;
                team1 = t1;
                team2 = t2;
                player_name1 = pn1;
                player_name2 = pn2;
                player_id1 = pi1;
                player_id2 = pi2;
            }
            InitializeComponent();
            lblTournamentName.Text = tournamentname;
        }

        private void frmPlaybyPlay_Load(object sender, EventArgs e)
        {
            getUserData();
            getMatchData();
            shuttle = Properties.Resources.shuttle;
            rect = new Rectangle(20, 20, 20, 20);
        }

        private void frmPlaybyPlay_Shown(object sender, EventArgs e)
        {
            if (matchtype == 2 || matchtype == 3)
            {
                byte[] image = null;
                int rowcount = 0;
                foreach (DataRow row in user_data.Rows)
                {
                    if (player_id1 == row.Field<int>(0))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals("")) { lblMDPlayer1.Text = player_name1; }
                        else
                        {
                            image = (byte[])(row.Field<byte[]>(42));
                            MemoryStream memstream = new MemoryStream(image);
                            pbxMDPlayer1.Image = Image.FromStream(memstream);
                            lblMDPlayer1.Text = player_name1;
                        }
                    }
                    if (player_id2 == row.Field<int>(0))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals("")) { lblMDPlayer2.Text = player_name1; }
                        else
                        {
                            image = (byte[])(row.Field<byte[]>(42));
                            MemoryStream memstream = new MemoryStream(image);
                            pbxMDPlayer2.Image = Image.FromStream(memstream);
                            lblMDPlayer2.Text = player_name2;
                        }
                    }
                    if (player_id3 == row.Field<int>(0))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals("")) { lblMDPlayer3.Text = player_name1; }
                        else
                        {
                            image = (byte[])(row.Field<byte[]>(42));
                            MemoryStream memstream = new MemoryStream(image);
                            pbxMDPlayer3.Image = Image.FromStream(memstream);
                            lblMDPlayer3.Text = player_name3;
                        }
                    }
                    if (player_id4 == row.Field<int>(0))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals("")) { lblMDPlayer4.Text = player_name1; }
                        else
                        {
                            image = (byte[])(row.Field<byte[]>(42));
                            MemoryStream memstream = new MemoryStream(image);
                            pbxMDPlayer4.Image = Image.FromStream(memstream);
                            lblMDPlayer4.Text = player_name4;
                        }
                    }
                    rowcount++;
                }
                lblTeam1.Text = team1;
                lblTeam2.Text = team2;
                pnlMixDouble.Show();
                pnlSingle.Hide();
                pbxMDPlayer1.Click += new EventHandler(pbxMDPlayer1_Click);
            }
            else if (matchtype == 1)
            {
                byte[] image = null;
                int rowcount = 0;
                foreach (DataRow row in user_data.Rows)
                {
                    if (player_id1 == row.Field<int>(0))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals(""))
                        {
                            lblSPlayer1.Text = player_name1;
                        }
                        else
                        {
                            image = (byte[])(row.Field<byte[]>(42));
                            MemoryStream memstream = new MemoryStream(image);
                            pbxSPlayer1.Image = Image.FromStream(memstream);
                            lblSPlayer1.Text = player_name1;
                        }
                    }
                    if (player_id2 == row.Field<int>(0))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals(""))
                        {
                            lblSPlayer2.Text = player_name2;
                        }
                        else
                        {
                            image = (byte[])(row.Field<byte[]>(42));
                            MemoryStream memstream = new MemoryStream(image);
                            pbxSPlayer2.Image = Image.FromStream(memstream);
                            lblSPlayer2.Text = player_name2;
                        }
                    }
                    rowcount++;
                }
                lblTeam1.Text = team1;
                lblTeam2.Text = team2;
                pnlSingle.Show();
                pnlMixDouble.Hide();
                pbxSPlayer1.Click += new EventHandler(pbxSPlayer1_Click);
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

        private void getMatchData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.match", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                match_data = new DataTable();
                sqlDataAdapter.Fill(match_data);
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

        private void pbxSCourt_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(shuttle, rect);
        }

        private void pbxSCourt_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.X + "\n" + e.Y);
            rect = new Rectangle(e.X, e.Y, 20, 20);
            pbxSCourt.Invalidate();
            if (e.X >= 69 && e.X <= 327)
            {
                if (e.Y >= 45 && e.Y <= 272)
                {
                    t2Score++;
                    lblScore2.Text = t2Score.ToString("00");
                    foreach (DataRow row in match_data.Rows)
                    {
                        if (row.Field<int>(2) == player_id1 && row.Field<int>(4) == player_id2)
                        {
                            play_id++;
                            MessageBox.Show(player_id1.ToString() + "\n" + player_id2.ToString());
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2, attack_type)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player11 = '" + player_id1 + "' AND player21 = '" + player_id2 + "'),'" + server + "', '" + play_id + "', '" +
                                           e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "','" + cbxAttackType.Text + "')";
                            saveData(query);
                        }
                    }
                }
            }
            if (e.X >= 327 && e.X <= 583)
            {
                if (e.Y >= 45 && e.Y <= 272)
                {
                    t1Score++;
                    lblScore1.Text = t1Score.ToString("00");
                    foreach (DataRow row in match_data.Rows)
                    {
                        if (row.Field<int>(2) == player_id1 && row.Field<int>(4) == player_id2)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2, attack_type)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player11 = '" + player_id1 + "' AND player21 = '" + player_id2 + "'),'" + server + "', '" + play_id + "', '" +
                                           e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "','" + cbxAttackType.Text + "')";
                            saveData(query);
                        }
                    }
                }
            }
        }

        private void pbxSPlayer1_Click(object sender, EventArgs e)
        {
            pbxSPlayer1.BorderStyle = BorderStyle.FixedSingle;
            pbxSPlayer2.BorderStyle = BorderStyle.None;
            server = player_id1;
        }

        private void pbxSPlayer2_Click(object sender, EventArgs e)
        {
            pbxSPlayer2.BorderStyle = BorderStyle.FixedSingle;
            pbxSPlayer1.BorderStyle = BorderStyle.None;
            server = player_id2;
        }

        private void pbxMDCourt_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(shuttle, rect);
        }

        private void pbxMDCourt_MouseClick(object sender, MouseEventArgs e)
        {
            rect = new Rectangle(e.X, e.Y, 20, 20);
            pbxMDCourt.Invalidate();
            if (e.X >= 35 && e.X <= 327)
            {
                if (e.Y >= 24 && e.Y <= 291)
                {
                    t2Score++;
                    lblScore2.Text = t2Score.ToString("00");
                    int rowcount = 0;
                    foreach (DataRow row in match_data.Rows)
                    {
                        int p1 = int.Parse(match_data.Rows[rowcount][2].ToString());
                        int p2 = int.Parse(match_data.Rows[rowcount][3].ToString());
                        int p3 = int.Parse(match_data.Rows[rowcount][4].ToString());
                        int p4 = int.Parse(match_data.Rows[rowcount][5].ToString());
                        if (p1 == player_id1 && p2 == player_id2 && p3 == player_id3 && p4 == player_id4)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2, attack_type)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player11 = '" + player_id1 + "' AND player12 = '" + player_id2 + "' AND player21 = '" + player_id3 +
                                           "' AND player22 = '" + player_id4 + "'),'" + server + "', '" + play_id + "', '" + e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "','" + cbxAttackType.Text + "')";
                            saveData(query);
                        }
                        rowcount++;
                    }
                }
            }
            if (e.X >= 327 && e.X <= 617)
            {
                if (e.Y >= 24 && e.Y <= 291)
                {
                    t1Score++;
                    lblScore1.Text = t1Score.ToString("00");
                    int rowcount = 0;
                    foreach (DataRow row in match_data.Rows)
                    {
                        int p1 = int.Parse(match_data.Rows[rowcount][2].ToString());
                        int p2 = int.Parse(match_data.Rows[rowcount][3].ToString());
                        int p3 = int.Parse(match_data.Rows[rowcount][4].ToString());
                        int p4 = int.Parse(match_data.Rows[rowcount][5].ToString());
                        if (p1 == player_id1 && p2 == player_id2 && p3 == player_id3 && p4 == player_id4)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2, attack_type)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player11 = '" + player_id1 + "' AND player12 = '" + player_id2 + "' AND player21 = '" + player_id3 +
                                           "' AND player22 = '" + player_id4 + "'),'" + server + "', '" + play_id + "','" + e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "','" + cbxAttackType.Text + "')";
                            saveData(query);
                        }
                        rowcount++;
                    }
                }
            }
        }

        private void pbxMDPlayer1_Click(object sender, EventArgs e)
        {
            pbxMDPlayer1.BorderStyle = BorderStyle.FixedSingle;
            pbxMDPlayer2.BorderStyle = BorderStyle.None;
            pbxMDPlayer3.BorderStyle = BorderStyle.None;
            pbxMDPlayer4.BorderStyle = BorderStyle.None;
            server = player_id1;
        }

        private void pbxMDPlayer2_Click(object sender, EventArgs e)
        {
            pbxMDPlayer2.BorderStyle = BorderStyle.FixedSingle;
            pbxMDPlayer1.BorderStyle = BorderStyle.None;
            pbxMDPlayer3.BorderStyle = BorderStyle.None;
            pbxMDPlayer4.BorderStyle = BorderStyle.None;
            server = player_id2;
        }

        private void pbxMDPlayer3_Click(object sender, EventArgs e)
        {
            pbxMDPlayer3.BorderStyle = BorderStyle.FixedSingle;
            pbxMDPlayer2.BorderStyle = BorderStyle.None;
            pbxMDPlayer1.BorderStyle = BorderStyle.None;
            pbxMDPlayer4.BorderStyle = BorderStyle.None;
            server = player_id3;
        }

        private void pbxMDPlayer4_Click(object sender, EventArgs e)
        {
            pbxMDPlayer4.BorderStyle = BorderStyle.FixedSingle;
            pbxMDPlayer2.BorderStyle = BorderStyle.None;
            pbxMDPlayer3.BorderStyle = BorderStyle.None;
            pbxMDPlayer1.BorderStyle = BorderStyle.None;
            server = player_id4;
        }

        private void btnChangeCourt_Click(object sender, EventArgs e)
        {
            if(t1Score > t2Score) { team1_counter++; }
            if(t2Score > t1Score) { team2_counter++; }
            int temp_playerid1, temp_playerid2;
            string temp_string, temp_player1, temp_player2;
            PictureBox pic1 = new PictureBox();
            PictureBox pic2 = new PictureBox();
            if (matchtype == 2 || matchtype == 3)
            {
                temp_string = team1;
                team1 = team2;
                team2 = temp_string;
                temp_player1 = player_name1;
                temp_player2 = player_name2;
                player_name1 = player_name3;
                player_name2 = player_name4;
                player_name3 = temp_player1;
                player_name4 = temp_player2;
                temp_playerid1 = player_id1;
                temp_playerid2 = player_id2;
                player_id1 = player_id3;
                player_id2 = player_id4;
                player_id3 = temp_playerid1;
                player_id4 = temp_playerid2;
                pic1.Image = pbxMDPlayer1.Image;
                pic2.Image = pbxMDPlayer2.Image;
                pbxMDPlayer1.Image = pbxMDPlayer3.Image;
                pbxMDPlayer2.Image = pbxMDPlayer4.Image;
                pbxMDPlayer3.Image = pic1.Image;
                pbxMDPlayer4.Image = pic2.Image;
                temp_player1 = lblMDPlayer1.Text;
                temp_player2 = lblMDPlayer2.Text;
                lblMDPlayer1.Text = lblMDPlayer2.Text;
                lblMDPlayer2.Text = lblMDPlayer3.Text;
                lblMDPlayer3.Text = temp_player1;
                lblMDPlayer4.Text = temp_player2;
            }
            else if (matchtype == 1)
            {
                temp_string = team1;
                team1 = team2;
                team2 = temp_string;
                temp_string = player_name1;
                player_name1 = player_name2;
                player_name2 = temp_string;
                temp_playerid1 = player_id1;
                player_id1 = player_id2;
                player_id2 = temp_playerid1;
                pic1.Image = pbxSPlayer1.Image;
                pbxSPlayer1.Image = pbxSPlayer2.Image;
                pbxSPlayer2.Image = pic1.Image;
                temp_player1 = lblSPlayer1.Text;
                lblSPlayer1.Text = lblSPlayer2.Text;
                lblSPlayer2.Text = temp_player1;
            }
        }

        private void btnFinishGame_Click(object sender, EventArgs e)
        {
            MessageBox.Show(matchtype.ToString());
            if (matchtype == 1)
            {
                string query = "UPDATE powersmash.match SET score_1 = '" + team1_counter.ToString() + "', score_2 = '" + team2_counter.ToString() + "' WHERE player11 = '" + player_id1.ToString() +
                               "' AND player21 = '" + player_id2 + "'";
                saveData(query);
            }
            if (matchtype == 2 || matchtype == 3)
            {
                string query = "UPDATE powersmash.match SET score_1 = '" + team1_counter.ToString() + "', score_2 = '" + team2_counter.ToString() + "' WHERE player11 = '" + player_id1.ToString() +
                               "' AND player12 = '" + player_id2 + "' AND player21 = '" + player_id3 + "' AND player22 = '" + player_id4 + "'";
                saveData(query);
            }
            //update query to the match score;
            //back to main form
            //string query = "UPDATE powersmash.match "

            frmMain main = new frmMain();
            main.Show();
            this.Dispose();
        }
    }
}
