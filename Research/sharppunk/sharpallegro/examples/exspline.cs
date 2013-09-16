using System;
using System.Text;

using sharpallegro;

namespace exspline
{
  class exspline : Allegro
  {
    public struct NODE
    {
      public int x, y;
      public int tangent;
    }


    const int MAX_NODES = 1024;

    static NODE[] nodes = new NODE[MAX_NODES];

    static int node_count;

    static int curviness;

    static bool show_tangents;
    static bool show_control_points;



    /* calculates the distance between two nodes */
    static int node_dist(NODE n1, NODE n2)
    {
      int SCALE = 64;

      int dx = itofix(n1.x - n2.x) / SCALE;
      int dy = itofix(n1.y - n2.y) / SCALE;

      return fixsqrt(fixmul(dx, dx) + fixmul(dy, dy)) * SCALE;
    }



    /* constructs nodes to go at the ends of the list, for tangent calculations */
    static NODE dummy_node(NODE node, NODE prev)
    {
      NODE n;

      n.x = node.x - (prev.x - node.x) / 8;
      n.y = node.y - (prev.y - node.y) / 8;
      n.tangent = itofix(0);

      return n;
    }



    /* calculates a set of node tangents */
    static void calc_tangents()
    {
      int i;

      nodes[0] = dummy_node(nodes[1], nodes[2]);
      nodes[node_count] = dummy_node(nodes[node_count - 1], nodes[node_count - 2]);
      node_count++;

      for (i = 1; i < node_count - 1; i++)
        nodes[i].tangent = fixatan2(itofix(nodes[i + 1].y - nodes[i - 1].y),
            itofix(nodes[i + 1].x - nodes[i - 1].x));
    }



    /* draws one of the path nodes */
    static void draw_node(int n)
    {
      circlefill(screen, nodes[n].x, nodes[n].y, 2, palette_color[1]);

      textprintf_ex(screen, font, nodes[n].x - 7, nodes[n].y - 7,
        palette_color[255], -1, string.Format("{0}", n));
    }



    /* calculates the control points for a spline segment */
    static void get_control_points(NODE n1, NODE n2, int[] points)//[8])
    {
      int dist = fixmul(node_dist(n1, n2), curviness);

      points[0] = n1.x;
      points[1] = n1.y;

      points[2] = n1.x + fixtoi(fixmul(fixcos(n1.tangent), dist));
      points[3] = n1.y + fixtoi(fixmul(fixsin(n1.tangent), dist));

      points[4] = n2.x - fixtoi(fixmul(fixcos(n2.tangent), dist));
      points[5] = n2.y - fixtoi(fixmul(fixsin(n2.tangent), dist));

      points[6] = n2.x;
      points[7] = n2.y;
    }



    /* draws a spline curve connecting two nodes */
    static void draw_spline(NODE n1, NODE n2)
    {
      int[] points = new int[8];
      int i;

      get_control_points(n1, n2, points);
      spline(screen, points, palette_color[255]);

      if (show_control_points)
        for (i = 1; i <= 2; i++)
          circlefill(screen, points[i * 2], points[i * 2 + 1], 2, palette_color[2]);
    }



    /* draws the spline paths */
    static void draw_splines()
    {
      int i;

      acquire_screen();

      clear_to_color(screen, makecol(255, 255, 255));

      textout_centre_ex(screen, font, "Spline curve path", SCREEN_W / 2, 8,
            palette_color[255], palette_color[0]);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, 32, palette_color[255],
         palette_color[0], string.Format("Curviness = {0:.2}",
         fixtof(curviness)));
      textout_centre_ex(screen, font, "Up/down keys to alter", SCREEN_W / 2, 44,
            palette_color[255], palette_color[0]);
      textout_centre_ex(screen, font, "Space to walk", SCREEN_W / 2, 68,
            palette_color[255], palette_color[0]);
      textout_centre_ex(screen, font, "C to display control points", SCREEN_W / 2,
            92, palette_color[255], palette_color[0]);
      textout_centre_ex(screen, font, "T to display tangents", SCREEN_W / 2, 104,
            palette_color[255], palette_color[0]);

      for (i = 1; i < node_count - 2; i++)
        draw_spline(nodes[i], nodes[i + 1]);

      for (i = 1; i < node_count - 1; i++)
      {
        draw_node(i);

        if (show_tangents)
        {
          line(screen, nodes[i].x - fixtoi(fixcos(nodes[i].tangent) * 24),
                 nodes[i].y - fixtoi(fixsin(nodes[i].tangent) * 24),
                 nodes[i].x + fixtoi(fixcos(nodes[i].tangent) * 24),
                 nodes[i].y + fixtoi(fixsin(nodes[i].tangent) * 24),
                 palette_color[1]);
        }
      }

      release_screen();
    }



    /* let the user input a list of path nodes */
    static void input_nodes()
    {
      clear_to_color(screen, makecol(255, 255, 255));

      textout_centre_ex(screen, font, "Click the left mouse button to add path " +
            "nodes", SCREEN_W / 2, 8, palette_color[255],
            palette_color[0]);
      textout_centre_ex(screen, font, "Right mouse button or any key to finish",
            SCREEN_W / 2, 24, palette_color[255], palette_color[0]);

      node_count = 1;

      show_mouse(screen);

      do
      {
        poll_mouse();
      } while (mouse_b != 0);

      clear_keybuf();

      for (; ; )
      {
        poll_mouse();

        if ((mouse_b & 1) > 0)
        {
          if (node_count < MAX_NODES - 1)
          {
            nodes[node_count].x = mouse_x;
            nodes[node_count].y = mouse_y;

            show_mouse(NULL);
            draw_node(node_count);
            show_mouse(screen);

            node_count++;
          }

          do
          {
            poll_mouse();
          } while ((mouse_b & 1) > 0);
        }

        if ((mouse_b & 2) > 0 || (keypressed()))
        {
          if (node_count < 3)
            alert("You must enter at least two nodes",
            null, null, "OK", null, 13, 0);
          else
            break;
        }
      }

      show_mouse(NULL);

      do
      {
        poll_mouse();
      } while (mouse_b > 0);

      clear_keybuf();
    }



    /* moves a sprite along the spline path */
    static void walk()
    {
      int MAX_POINTS = 256;

      int[] points = new int[8];
      int[] x = new int[MAX_POINTS], y = new int[MAX_POINTS];
      int n, i;
      int npoints;
      int ox, oy;

      acquire_screen();

      clear_to_color(screen, makecol(255, 255, 255));

      for (i = 1; i < node_count - 1; i++)
        draw_node(i);

      release_screen();

      do
      {
        poll_mouse();
      } while (mouse_b > 0);

      clear_keybuf();

      ox = -16;
      oy = -16;

      xor_mode(TRUE);

      for (n = 1; n < node_count - 2; n++)
      {
        npoints = (fixtoi(node_dist(nodes[n], nodes[n + 1])) + 3) / 4;
        if (npoints < 1)
          npoints = 1;
        else if (npoints > MAX_POINTS)
          npoints = MAX_POINTS;

        get_control_points(nodes[n], nodes[n + 1], points);
        calc_spline(points, npoints, x, y);

        for (i = 1; i < npoints; i++)
        {
          vsync();
          acquire_screen();
          circlefill(screen, ox, oy, 6, palette_color[2]);
          circlefill(screen, x[i], y[i], 6, palette_color[2]);
          release_screen();
          ox = x[i];
          oy = y[i];

          poll_mouse();

          if ((keypressed()) || (mouse_b > 0))
            goto getout;
        }
      }

    getout:

      xor_mode(FALSE);

      do
      {
        poll_mouse();
      } while (mouse_b > 0);

      clear_keybuf();
    }

    /* main program */
    static int Main()
    {
      int c;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_mouse();
      install_timer();

      if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 640, 480, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message(string.Format("Unable to set any graphic mode\n{0}\n",
              allegro_error));
          return 1;
        }
      }

      set_palette(desktop_palette);

      input_nodes();
      calc_tangents();

      curviness = ftofix(0.25);
      show_tangents = false;
      show_control_points = false;

      draw_splines();

      for (; ; )
      {
        if (keypressed())
        {
          c = readkey() >> 8;
          if (c == KEY_ESC)
            break;
          else if (c == KEY_UP)
          {
            curviness += ftofix(0.05);
            draw_splines();
          }
          else if (c == KEY_DOWN)
          {
            curviness -= ftofix(0.05);
            draw_splines();
          }
          else if (c == KEY_SPACE)
          {
            walk();
            draw_splines();
          }
          else if (c == KEY_T)
          {
            show_tangents = !show_tangents;
            draw_splines();
          }
          else if (c == KEY_C)
          {
            show_control_points = !show_control_points;
            draw_splines();
          }
        }
      }

      return 0;
    }
  }
}
