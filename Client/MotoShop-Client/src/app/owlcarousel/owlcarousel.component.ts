import { Component, Input, OnInit } from '@angular/core';
import { OwlOptions } from 'ngx-owl-carousel-o';

@Component({
  selector: 'app-owlcarousel',
  templateUrl: './owlcarousel.component.html',
  styleUrls: ['./owlcarousel.component.sass']
})
export class OWLCarouselComponent implements OnInit {

  constructor() { }

  @Input() Images: string[];

  owlOptions: OwlOptions = {
    loop: true,
    mouseDrag: false,
    touchDrag: false,
    pullDrag: false,
    dots: false,
    navSpeed: 700,
    navText: ['', ''],
    responsive: {

    },
    nav: true
  }

  ngOnInit(): void {
  }

}
