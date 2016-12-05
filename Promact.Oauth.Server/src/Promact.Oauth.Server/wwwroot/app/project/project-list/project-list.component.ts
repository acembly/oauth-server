﻿import { Component, OnInit } from "@angular/core";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { Router } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoginService } from '../../login.service';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: 'app/project/project-list/project-list.html',
    

})
export class ProjectListComponent implements OnInit {
    projects: Array<ProjectModel>;
    project: ProjectModel;
    admin: boolean;
    constructor(private router: Router, private projectService: ProjectService, private toast: Md2Toast, private loginService: LoginService,
        private loader: LoaderService, private userRole: UserRole, private stringConstant: StringConstant) {
        this.projects = new Array<ProjectModel>();
        this.project = new ProjectModel();
    }
    getProjects() {
        this.loader.loader = true;
        this.projectService.getProjects().subscribe((projects) => {
            this.projects = projects;
            this.loader.loader = false;
        }, err => {
            this.toast.show('Project list is empty.');
            this.loader.loader = false;
        });
    }
    ngOnInit() {
        if (this.userRole.Role === this.stringConstant.admin) {
            this.admin = true;}
        else {this.admin = false;}
        this.getProjects();
    }
    editProject(Id) {
        this.router.navigate([this.stringConstant.projectEdit, Id]);
    }
    viewProject(Id) {
        this.router.navigate([this.stringConstant.projectView, Id]);
    }
}