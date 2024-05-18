import { DatePipe } from '@angular/common';

const DATE_PIPE = new DatePipe("en-US", "Europe/Kyiv");
const DATE_FORMAT: string = 'yyyy-MM-dd hh:mm:ss a';

export const toLocalTime = (value: string): Date => {
  return new Date(DATE_PIPE.transform(value, DATE_FORMAT) ?? '');
}
