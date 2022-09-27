import { v4 as uuidv4 } from 'uuid';

export class Note {
    public id: string;
    public title: string;
    public content: string;

    constructor(title: string, content: string) {
        this.id = uuidv4();
        this.title = title;
        this.content = content;
    }
}