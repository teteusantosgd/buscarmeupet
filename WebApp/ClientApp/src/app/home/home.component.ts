import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { GeminiResponse } from '../models/gemini-response';
import { Pet } from '../models/pet';
import { PetsService } from '../services/pets.service';
import { PetsFilter } from '../models/pets-filter';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  selectedFile: File | null = null;
  loading = false;
  previewImage: any = null;
  petDescription: Pet | null = null;
  pets: Pet[] = [];

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient, 
    private sanitizer: DomSanitizer,
    private petsService: PetsService) { }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];

    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.previewImage = this.sanitizer.bypassSecurityTrustUrl(e.target.result); // Sanitize the URL
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  find() {
    console.log('Selected file:', this.selectedFile)

    if (!this.selectedFile) {
      return;
    }

    this.loading = true;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.http.post<GeminiResponse>(this.baseUrl + 'search', formData)
      .subscribe({
        next: async(response) => {
          let petDescriptionText = response.candidates[0]?.content?.parts[0]?.text;
          petDescriptionText = petDescriptionText 
            .replace(/'/g, '"') 
            .replace(/\s+/g, ' ')
            .replace(/,$/, '')
            .replace('```json', '')
            .replace('```', '')
            .replace('\n', '');
          this.petDescription = JSON.parse(petDescriptionText ?? '{}');

          let filter = this.petDescription as PetsFilter;
          filter.tipo = 'cachorro';
          filter.raca = filter.raca?.replace(filter.cor, '')
          this.pets = await firstValueFrom(this.petsService.get(filter));
                                                                                                                                                             (this.petDescription);
          this.loading = false;
        },
        error: (error) => {
          console.error('Upload error:', error);
          this.loading = false;
        }
      })
  }
}