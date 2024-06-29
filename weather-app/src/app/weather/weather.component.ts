import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { injectQuery } from '@tanstack/angular-query-experimental';
import { lastValueFrom } from 'rxjs';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-weather',
  standalone: true,
  templateUrl: './weather.component.html',
  imports: [CommonModule],
})
export class WeatherComponent {
  http = inject(HttpClient);

  query = injectQuery(() => ({
    queryKey: ['weatherData'],
    queryFn: () =>
      lastValueFrom(
        this.http.get<any[]>('http://localhost:5041/weatherforecast')
      ),
  }));
}
