import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BuildingBlocksService
{

  constructor ()
  {
  }

  public searchTree (node, id)
  {
    if (node.Id == id)
    {
      return node;
    } else if (node.Children != null)
    {
      let result = null;
      for (let i = 0; result == null && i < node.Children.length; i++)
      {
        result = this.searchTree(node.Children[i], id);
      }
      return result;
    }

    return null;
  }

  public updateTree (parentNode, id, updatedNode)
  {
    if (parentNode.Id == id)
    {
      // console.log("updating node");
      // parentNode.assign(updatedNode)
      Object.assign(parentNode,updatedNode);
      return true;

    } else if (parentNode.Children != null)
    {
      for (let i = 0; i < parentNode.Children.length; i++) {
        this.updateTree(parentNode.Children[i], id, updatedNode);
      }
    }
  }
}
