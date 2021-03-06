export declare type FieldAppearanceRenderingType = "Custom" | "Web" | "Predefined";
export declare type OpenParameters = {
    headers: {
        [header: string]: string;
    };
    withCredentials: boolean;
    password: string;
};
export declare type OptionalContentConfig = {
    creator: null | string;
    name: null | string;
    getGroup(id: string): OptionalContentGroup;
    getGroups(): {
        [id: string]: OptionalContentGroup;
    };
    getOrder(): {
        name: string | null;
        order: string[];
    }[];
    isVisible(groupOrId: string | OptionalContentGroup): any;
    setVisibility(id: string, visible: boolean): any;
};
export declare type OptionalContentGroup = {
    id: string;
    name: string;
    type: "OCG" | string;
    visible: boolean;
    exportState?: "ON" | "OFF";
    viewState?: "ON" | "OFF";
    printState?: "ON" | "OFF";
    creatorInfo?: {
        Creator: string;
        Subtype: string;
    };
    intent?: "View" | "Design";
    zoom?: {
        min?: number;
        max?: number;
    };
};
export declare type StructTreeNode = {
    role: "Root" | string;
    children: Array<StructTreeNode | StructTreeContent>;
};
export declare type StructTreeContent = {
    id: string;
    type: "content" | "object";
};
export declare type ViewerFeatureName = 'JavaScript' | 'AllAttachments' | 'FileAttachments' | 'SoundAttachments' | 'DragAndDrop' | 'SubmitForm' | 'Print';
export declare type StampCategory = {
    id: string;
    name: string;
    stampImages: string[];
    stampImageUrls?: string[];
    isDynamic: boolean;
    dpi: number;
};
