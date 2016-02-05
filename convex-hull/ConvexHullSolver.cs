using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        // Testing helper methods
        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void DrawAllPointsInOrder(List<System.Drawing.PointF> pointList, int milliseconds)
        {
            Pen black = new Pen(Color.Black, 1F);
            for(int i = 0; i < pointList.Count-1; i++) {
                g.DrawLine(black, pointList[i], pointList[i+1]);
                if(milliseconds > 0)
                    Pause(milliseconds);
            }
            g.DrawLine(black, pointList[pointList.Count - 1], pointList[0]);
            Refresh();
        }


        // Implementation
        public void Solve(List<System.Drawing.PointF> pointList)
        {
            // Order the points from xmin to xmax
            pointList.Sort((one, two) => one.X.CompareTo(two.X));
            DrawAllPointsInOrder(pointList, 1000);
            List<PointF> edgePointsInCCOrder = DivideAndConquer(pointList);
            DrawAllPointsInOrder(pointList, 1000);
        }

        private List<PointF> DivideAndConquer(List<PointF> points)
        {
            if (points.Count == 2 || points.Count == 3)
            {
                // TODO make sure they are in CC order starting with the left-most
                return points;
            }

            // Find the middle index
            int middleIdx = points.Count / 2;
            if(points.Count % 2 == 1)
                middleIdx++;

            // DAC the left and right ranges of the points
            List<PointF> edgePtsLeft = DivideAndConquer(points.GetRange(0, middleIdx));
            List<PointF> edgePtsRight = DivideAndConquer(points.GetRange(middleIdx, points.Count - middleIdx));

            // Get upper and lower tangent indexes
            Tuple<int, int> upperTangentIndexes = FindUpperTangent();
            Tuple<int, int> lowerTangentIndexes = FindLowerTangent();
            int idxOfUpperTangentInEdgePtsL = upperTangentIndexes.Item1;
            int idxOfUpperTangentInEdgePtsR = upperTangentIndexes.Item2;
            int idxOfLowerTangentInEdgePtsL = lowerTangentIndexes.Item1;
            int idxOfLowerTangentInEdgePtsR = lowerTangentIndexes.Item2;

            // Combine the new hulls using the newly found indexes
            List<PointF> combinedEdgePts = new List<PointF>();
            return combinedEdgePts;
        }

        private Tuple<int, int> FindUpperTangent(List<PointF> edgePtsL, List<PointF> edgePtsR)
        {
            // Please note that lists should be ordered clockwise with the left-most point at position 0.
            int idxLastCandidateFromPtsL = GetIdxOfRightMostPointInSet(edgePtsL);
            int idxLastCandidateFromPtsR = 0;

            while(true) 
            {
                // Current candidate from the set of points on the right
                int idxCurrCandidateFromPtsR = -1;
                while(true)
                {
                    int idxNewCandidateFromPtsR = FindRightUpperTangentCandidate(edgePtsL, edgePtsR, idxLastCandidateFromPtsL, idxLastCandidateFromPtsR);
                    if(idxNewCandidateFromPtsR == idxCurrCandidateFromPtsR) { break; } else { idxCurrCandidateFromPtsR = idxNewCandidateFromPtsR; }
                }

                int idxCurrCandidateFromPtsL = -1;
                while(true)
                {
                    int idxNewCandidateFromPtsL = FindLeftUpperTangentCandidate(edgePtsL, edgePtsR, idxLastCandidateFromPtsL, idxLastCandidateFromPtsR);
                    if(idxNewCandidateFromPtsL == idxCurrCandidateFromPtsL) { break; } else { idxCurrCandidateFromPtsR = idxNewCandidateFromPtsL; }
                }

                if(idxCurrCandidateFromPtsL == idxLastCandidateFromPtsL && idxCurrCandidateFromPtsR == idxLastCandidateFromPtsR) 
                {
                    return Tuple.Create(idxLastCandidateFromPtsL, idxLastCandidateFromPtsR);
                } else {
                    idxLastCandidateFromPtsL = idxCurrCandidateFromPtsL;
                    idxLastCandidateFromPtsR = idxCurrCandidateFromPtsR;
                }

            }

            return null;
        }

        private int GetIdxOfRightMostPointInSet(List<PointF> points)
        {
            float highestXVal = -1;
            int idxOfHighestXVal = -1;
            for(int i = 0; i < points.Count; i++)
            {
                PointF p = points[i];
                if (p.X > highestXVal)
                {
                    highestXVal = p.X;
                    idxOfHighestXVal = i;
                }
            }

            return idxOfHighestXVal;
        }

    }
}
