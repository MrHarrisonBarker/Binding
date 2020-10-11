export enum BlockType
{
  Text,
  Heading,
  SubHeading,
  Code
}

export namespace BlockType {

  export function values() {
    return Object.keys(BlockType).filter(
      (type) => isNaN(<any>type) && type !== 'values'
    );
  }
}
