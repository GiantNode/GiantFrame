﻿using EpPathFinding;
using Giant.Data;
using System.Collections.Generic;

namespace Server.Map
{
    public class Map
    {
        private DynamicGrid dynamicGrid;
        private JumpPointParam jumpPointParam;

        public MapModel Model { get; private set; }
        public int MapId => Model.MapId;

        public MapMananger MapMamanger { get; private set; }

        public Map(MapMananger mamanger, MapModel model)
        {
            this.MapMamanger = mamanger;
            this.Model = model;

            this.dynamicGrid = MapGridPosManager.GetGrid(model.BianryName);
            this.jumpPointParam = new JumpPointParam(this.dynamicGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never, HeuristicMode.EUCLIDEAN);
        }

        public List<GridPos> PathFind(GridPos startPos, GridPos endPos)
        {
            jumpPointParam.Reset(startPos, endPos);
            return JumpPointFinder.FindPath(jumpPointParam);
        }
    }
}
