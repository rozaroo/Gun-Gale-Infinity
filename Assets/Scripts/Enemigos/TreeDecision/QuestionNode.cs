using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : ITreeNode
{
    Func<bool> _question;
    ITreeNode _fNode;
    ITreeNode _tNode;
    
    public QuestionNode(Func<bool> question, ITreeNode tNode, ITreeNode fNode)
    {
        _question = question;
        _tNode = tNode;
        _fNode = fNode;
    }
    public QuestionNode(Func<bool> question, ITreeNode tNode)
    {
        _question = question;
        _tNode = tNode;

    }
    public void Execute()
    {
        if (_question()) 
        {
            if (_tNode != null) _tNode.Execute();
        }
        else 
        {
            if (_fNode != null) _fNode.Execute();
        }
        
    }
}
