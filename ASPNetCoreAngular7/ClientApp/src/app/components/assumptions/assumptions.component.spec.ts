import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssumptionsComponent } from './assumptions.component';

describe('AssumptionsComponent', () => {
    let component: AssumptionsComponent;
    let fixture: ComponentFixture<AssumptionsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AssumptionsComponent ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AssumptionsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should have h1 tag as \'Project Assumptions\'', async(() => {
        fixture.detectChanges();
        const compile = fixture.debugElement.nativeElement;
        const h1tag = compile.querySelector('h1');
        expect(h1tag.textContent).toBe('Project Assumptions');
    }));
});
