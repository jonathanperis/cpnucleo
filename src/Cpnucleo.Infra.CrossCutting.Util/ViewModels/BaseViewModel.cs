﻿using MessagePack;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

[MessagePackObject(true)]
public abstract class BaseViewModel
{
    [System.ComponentModel.DataAnnotations.Key]
    [Display(Name = "Id")]
    public Guid Id { get; set; }

    [Display(Name = "Data de Inclusão")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DataInclusao { get; set; }
}