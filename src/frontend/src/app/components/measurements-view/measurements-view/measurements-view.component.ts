import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/users.service';

@Component({
  selector: 'app-measurements-view',
  templateUrl: './measurements-view.component.html',
  styleUrls: ['./measurements-view.component.scss']
})
export class MeasurementsViewComponent implements OnInit {
  public users: User[] = [
    { Id: 1, FirstName: 'Steve', LastName: 'Jobs'},
    { Id: 2, FirstName: 'Paul', LastName: 'McCartney'},
    { Id: 3, FirstName: 'Julia', LastName: 'Roberts'}
  ];

  constructor(private readonly userService: UserService) {
  }

  public ngOnInit(): void {
    this.userService.getUsers().pipe(first()).subscribe((response: User[]) => {
      this.users.push(...response);
    });
  }
}
