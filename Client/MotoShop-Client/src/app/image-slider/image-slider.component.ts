import { DOCUMENT } from '@angular/common';
import { AfterContentInit, AfterViewInit, Component, ElementRef, Inject, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrls: ['./image-slider.component.sass']
})
export class ImageSliderComponent implements AfterViewInit {
  @Input() ImagesUrls: string[] = [];
  @Input() ShowNavigationMenu: boolean = true;
  @Input() Width: string | null = null;
  @Input() Height: string | null = null;

  constructor(private crDocument: ElementRef<HTMLDocument>) { }

  public images: NodeListOf<Element>;

  private imagesDictionary: Array<any> = []
  private currentImageIndex: number = 0;

  private totalImages: number;

  ngAfterViewInit() {
    this.images = this.crDocument.nativeElement.querySelectorAll(".img");

    if (this.Width) {
      let container = this.crDocument.nativeElement.querySelector(".image_Slider-container") as HTMLElement;

      container.style.width = this.Width
    }

    if (this.Height) {
      let container = this.crDocument.nativeElement.querySelector(".image_Slider-container") as HTMLElement;

      container.style.height = this.Height
    }

    this.totalImages = this.images.length;
    this.fetchImagesToDictionary();

    //display first image
    this.switchImage(this.currentImageIndex);
  }


  switchImage(index) {
    for (let i = 0; i < this.imagesDictionary.length; i++)
      if (this.imagesDictionary[i].index == index)
        this.imagesDictionary[i].element.classList.add("img-active");
      else
        this.imagesDictionary[i].element.classList.remove("img-active");

    this.currentImageIndex = index;

    let navigationBtn = this.crDocument.nativeElement.querySelector(`#nav-${index}`);

    if (navigationBtn) {

      this.switchNavigationButton(navigationBtn);
    }
  }

  fetchImagesToDictionary() {
    for (let i = 0; i < this.images.length; i++) {
      this.imagesDictionary.push({ index: i, element: this.images[i] })
    }
  }

  nextImg() {
    if (this.currentImageIndex == this.totalImages - 1)
      return;

    this.switchImage(this.currentImageIndex += 1);
  }

  previousImg() {
    if (this.currentImageIndex == 0)
      return;

    this.switchImage(this.currentImageIndex -= 1);
  }

  switchNavigationButton(control: Element) {
    let all = this.crDocument.nativeElement.querySelectorAll(".nav-btn");

    for (let i = 0; i < all.length; i++) {
      all[i].classList.remove("active");
    }

    control.classList.add("active")
  }
}
