import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'order'
})
export class OrderPipe implements PipeTransform
{

  transform (value: any[], by: string): any[]
  {
    console.log(value);
    value.sort(((a:any, b:any) => {
      let aO = a[by];
      let bO = b[by];

      if (aO < bO) {
        return -1
      }
      if (bO > aO) {
        return 1;
      }
      return 0;
    }))

    return value
  }

}
