using UnityEngine;
using System.Collections.Generic;
using System;

public static class ThetaStar {
	static float Heuristic(MapNode node, MapNode end) { 
        return Vector3.Distance(node.transform.position, end.transform.position); ;
    }

	static MapNode RemoveBest(HashSet<MapNode> nodes, Dictionary<MapNode, float> heuristicCosts) { 
        float minCost = float.PositiveInfinity;
        MapNode minMapNode = null;
        foreach (var node in nodes) {
            var cost = heuristicCosts[node];
            if (cost < minCost) {
                minMapNode = node;
                minCost = cost;
            }
        }
        nodes.Remove(minMapNode);
        return minMapNode;
	}

    static public Stack<MapNode> Run(MapNode start, MapNode end,LayerMask layersOfRaycast) { 
        var open = new HashSet<MapNode>();
        var closed = new HashSet<MapNode>();
        var gs = new Dictionary<MapNode, float>();
        var fs = new Dictionary<MapNode, float>();
        var previous = new Dictionary<MapNode, MapNode>();

        //gs es el costo real del path hasta el momento
        gs[start] = 0f;
        //fs es el costo tentativo del path a los siguientes adyacentes del current, todos estos se pushean y despues se saca el mejor de estos en removebest
        fs[start] = 0f;
        previous[start] = null;
        bool success = false;
        int watchdog = 30000;

        open.Add(start);
        while (open.Count > 0) {
            var current = RemoveBest(open, fs);

            watchdog--;
            if (watchdog <= 0)
                throw new Exception("cagaste en estrella");

            if (current == end) {
                success = true;
                break;
            }

            closed.Add(current);

            foreach (var adj in current.adjacent) {

                if (closed.Contains(adj))
                    continue;
                
                var altCost = gs[current] + Vector3.Distance(current.transform.position, adj.transform.position); ;
                
                if (!gs.ContainsKey(adj) || altCost < gs[adj]) {
                    gs[adj] = altCost;
                    fs[adj] = altCost + Heuristic(adj, end);
                    if (lineOfSight(previous[current], adj, layersOfRaycast))
                        previous[adj] = previous[current];
                    else previous[adj] = current;
                    open.Add(adj);
                }
            }
        }

        Stack<MapNode> _path = new Stack<MapNode>();

        if (success) {
            var current = end;
            while (current != null) {
                _path.Push(current);
                current = previous[current];
            }
        }
		return _path;
	}

    private static bool lineOfSight(MapNode previusCurrent,MapNode adj, LayerMask layersOfRaycast) {
        if (previusCurrent == null)
            return false;

        var direction = adj.transform.position - previusCurrent.transform.position;
        var distance = Vector3.Distance(previusCurrent.transform.position, adj.transform.position);
        Debug.DrawRay(previusCurrent.transform.position, direction, Color.green, 1f);
        //RaycastHit rh;
        if (!Physics.Raycast(previusCurrent.transform.position, direction, distance, layersOfRaycast))
            return true;
        else return false; 
    }
}