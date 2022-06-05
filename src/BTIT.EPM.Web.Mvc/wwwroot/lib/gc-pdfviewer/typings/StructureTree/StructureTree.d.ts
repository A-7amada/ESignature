/// <reference path="../vendor/react/react.d.ts" />
//@ts-ignore
import { Component } from 'react';
import GcPdfViewer from '..';
import { StructTreeContent, StructTreeNode } from '../Models/ViewerTypes';
import { StructureTreeModel } from './types';
export declare type StructureTreeProps = {
    viewer: GcPdfViewer;
};
export declare class StructureTree extends Component<StructureTreeProps, StructureTreeModel> {
    private _mounted;
    componentDidMount(): void;
    componentWillUnmount(): void;
    navigateStructTreeContent(structTreeContent: StructTreeContent, pageIndex: number): void;
    toggleStructTreeNodeExpanded: (itemUid: string) => () => void;
    protected _onPageChevronButtonClick: (pageIndex: number) => () => void;
    protected _onPageButtonClick(pageIndex: number): void;
    navigatePage(pageIndex: number): void;
    countStructTreeNodeChildren(structTreeNode: StructTreeNode): number;
    private renderStructureTreeNode;
    renderStructureContentNode(structTreeContent: StructTreeContent, pageIndex: number): JSX.Element;
    render(): JSX.Element;
    private _consumedClickTime;
    consumeClickEvent(): void;
    get isClickEventConsumed(): boolean;
}
