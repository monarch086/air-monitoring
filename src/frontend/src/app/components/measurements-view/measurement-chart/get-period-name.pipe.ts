import { Pipe, PipeTransform } from '@angular/core';
import { PERIOD_ITEMS } from '../measurements-view/measurement-view.constants';

@Pipe({
  name: 'getPeriodName'
})
export class GetPeriodNamePipe implements PipeTransform {
  transform(days: number | string): string {
    return PERIOD_ITEMS.find(i => i.value === Number(days))?.name ?? 'N/A';
  }
}
