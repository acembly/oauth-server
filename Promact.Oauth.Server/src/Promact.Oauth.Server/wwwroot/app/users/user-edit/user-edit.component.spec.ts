﻿declare let describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { UserModel } from '../../users/user.model';
import { UserEditComponent } from "../user-edit/user-edit.component";
import { UserService } from "../user.service";
import { UserModule } from '../user.module';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockUserService } from "../../shared/mocks/user/mock.user.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { UserRole } from "../../shared/userrole.model";

describe("User Edit Test", () => {
    let userService: UserService;
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: UserRole, useClass: UserRole }
            ]
        }).compileComponents();

    }));

    it("should get particular user details", () => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component     
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: "1" };
        let userEditComponent = fixture.componentInstance;
        let expectedFirstName = "First Name";
        userEditComponent.ngOnInit();
        expect(userEditComponent.user.FirstName).toBe(expectedFirstName);
    });


    it("should check User first name before update", () => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component            
        let userEditComponent = fixture.componentInstance;
        let expectedFirstName = "First Name";
        let userModel = new UserModel();
        userModel.FirstName = expectedFirstName;
        userEditComponent.editUser(userModel);
        expect(userModel.FirstName).toBe(expectedFirstName);
    });

});